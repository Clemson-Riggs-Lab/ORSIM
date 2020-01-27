using UnityEngine;
using System.Collections;

/**
 * Original Author: Julian Dixon
 * Last Modified: July 16 2016
 * 
 * This script provides a graph manager suited to create an electrocardiogram
 * 
 * This class serves as a good baseline for understanding how I've done the graphs
 * since this is the first one I completed, the documentation and comments are 
 * much more extensive in this file. The rest of the graphs should be fairly
 * straight forward if you understand this one
 * 
 */
public class ECGManager : GraphManager {
    // patient singleton to load the default and update values
    Patient patient;

    public bool triggerSANode = false; 				// 	Whether we need to stimulate the atria
    private bool triggerAVNode = false; 			// 	Whether we need to stimulate the ventricles
    public bool stateAsystole = false;				//	Whether the heart is working (true) or stopped (false)
    public bool arhythmia = false;

    //	Heart voltage values for the formulation of the ECG trace

    private float voltageP;							// 	Atrial contraction voltage
    private float voltageQRS;						// 	Ventricular contraction
    private float voltageT;							// 	Ventricular relaxation (repolarisation)
    private float voltageU;							// 	Purkinjee repolarisation
    private float voltageOsborn;                    //	Osborn wave

    private ContinousMonitor continuousMonitor;

    // flag to tell if we are on the first beat or not
    private bool firstBeat;

    // public so its easily viewd in the editor
    public float intervalR = 1.0f;
    // Array of the last 10 RR intervals from which we determine the heartrate
    private float[] intervalArrayR;

    // previous and current interval times
    private float previousR;
    private float currentR;

    // heart rate in beats per minute
    public float heartRate;

    // variables controling the portions of the wave where there is no voltage
    private float PRseglen = 0.09f;
    private float segmentST;

    // variables controlling how quickly the trace moves
    // during each of the waves
    private float Pwavrate;
    private float Qrswavrate;
    private float Jwavrate;
    private float Uwavrate;
    private float Twavrate;

    int duration = 2;
    // invertUwave, the U wave is not present in all leads, an inversion of 2.0f
    // keeps the line flat
    public float invertUWave = 2.0f;

    // if you want to add a J wave, make this number positive
    public float addJWave = 0.0f;

    // voltages are different in different EKG leads
    // use the multiplier to control this
    public float voltageMultiplier;

    // variables to measure how long each wave is taking
    public float Ptime, Qrstime, Ttime, Utime;

    /*
     * Here we receive an event as a parameter, and determine
     * how it should be handled
     */
    public override void HandleTimeEvent(TimeEvent te)
    {
        // get event type and set the event occuring flag
        string t = te.evtType;
        eventOccuring = true;
        duration = (int)(te.endTime - te.startTime).TotalSeconds;
        // choose which thread we need to start
        if (t == "change")
        {
            StartCoroutine(MoveHeartRateTowards(te.factor));
        }
        else if (t == "stabilize")
        {
            StartCoroutine(MoveHeartRateTowards(patient.baseHeartRate));
           // StartCoroutine(Stabilize());           
        }
        else if (t == "leadsoff")
        {
            StartCoroutine(LeadsOff());
        }
        else if (t == "arhythmia")
        {
            // set arhythmia flag and update the patient singleton arhythmia flag
            arhythmia = true;
            patient.arhythmia = true;
            // NOTE: Arhythmia does not work properly
            StartCoroutine(Arhythmia());
        }
            

    }

    // set the boolean flags so that the first Update() starts the correct threads
    void Awake()
    {
        triggerSANode = false;
        triggerAVNode = false;
        eventOccuring = false;
        firstBeat = true;
    }

	// Use this for initialization
	void Start () {
        // get the patient singleton and default values
        patient = Patient.Instance;
        heartRate = patient.HeartRate;
        intervalR = heartRate / 60.0f;
        continuousMonitor = GameObject.FindGameObjectWithTag("ContMonitor").GetComponent<ContinousMonitor>();


        triggerSANode = true;		// start new heartbeat 

        // set the rates for the waves
        Pwavrate = 12.5f;
        Qrswavrate = 25.0f;
        Jwavrate = 25.0f;
        Twavrate = 4.6f;
        Uwavrate = 25.0f;

        // create the interval array
        intervalArrayR = new float[10];
        for (var i = 0; i < 10; i++)
            intervalArrayR[i] = intervalR;

	}

    // regular update function controls the actual heartbeat
    // values that are accessed by other classes are handled in FixedUpdate
    void Update()
    {
        if (stateAsystole)	//	If the heart is stopped then stop all voltage output from all the heart's chambers
        {
            triggerSANode = false;			//	No atrial contraction
            triggerAVNode = false;			//	No ventricular contraction

            voltageP = 0.0f;                // set all voltages to 0
            voltageQRS = 0.0f;
            voltageT = 0.0f;
            voltageOsborn = 0.0f;
            voltageU = 0.0f;
        }
        else                //	If the heart is not stopped see if we have atrial or ventricular stimulation
        {
            if (triggerSANode)				// Do we need to start a new heartbeat
            {
                StartCoroutine(PWave());			// Trigger Atrial Contraction	
                triggerSANode = false;              // prevents starting a new Pwave 
            }
            if (triggerAVNode)
            {
                StartCoroutine(QRSWave());
                triggerAVNode = false;              // prevents starting a new QRS wave
                                                    // before the heart beat is complete
            }

        }

    }

    // update variables accessed by other classes
    void FixedUpdate()
    {
        // save our previous sum
        previousSum = currentSum;

        // get our new current sum
        // total voltage across all chambers
        currentSum = voltageP + voltageQRS + voltageT + voltageU + voltageOsborn; 
        // currentSum += (-0.5f + Random.value);    // uncomment this if you want random variation

        // update heart rate in the patient singleton
        //patient.HeartRate = heartRate;///

        // apply voltage multiplier if we need it
        if (voltageMultiplier > 0.0f)
            currentSum *= voltageMultiplier;
    }

    // thread to calculate how long the last heart beat took
    // and calculate the overall heart rate as an average of the last 10 heart beats
    public IEnumerator RInterval()
    {
        float avgInterval = 0.0f;
        for (var n = 0; n < 9; n++)
        {
           intervalArrayR[n] = intervalArrayR[n + 1];      // We move the last 10 P interval readings and ...
            avgInterval += intervalArrayR[n];                   // Total all the P intervals
        }
        intervalArrayR[9] = intervalR;                      // Add this one to the array
        avgInterval += intervalArrayR[9];                       // Add this one to the total
        avgInterval /= 10.0f;                               // calculate the average

        // (1 beat / avgInterval sec) * (60 sec/min)
        heartRate = (1.0f / avgInterval) * 60.0f;
        // wait for the average interval time before this gets called again
        yield return new WaitForFixedUpdate();
    }

    // the first part of the heart beat waveform
    // represents the atrial contraction
    public IEnumerator PWave()			
    {
        float i = 0.0f;
        // start of p wave
        float start = Time.time;

        // move from 0.0 to 1.5 at a given rate
        // formula here is time = 1 / rate
        // so here the step will take 0.08 s = 1 / 12.5
        while (i < 1.0f)
        {
            i += Time.deltaTime * Pwavrate;
            voltageP = Mathf.SmoothStep(0.0f, 1.5f, i);
            yield return new WaitForFixedUpdate();
        }

        i = 0.0f;
        // move from 1.5 back to 0 -- again in 0.08 seconds
        while (i < 1.0f)
        {
            i += Time.deltaTime * Pwavrate;
            voltageP = Mathf.SmoothStep(1.5f, 0.0f, i);
            yield return new WaitForFixedUpdate();
        }

        // Pwave time by now should be about 0.16 s

        // this is effectively the PR segment ie 0.05 to 0.12 secs
        // total time += 0.09
        yield return new WaitForSeconds(PRseglen);
        
        // so now we should be somewhere around 0.25 total time

        // calculate time of Pwave with current time - start time
        Ptime = Time.time - start;



        // trigger ventricle contraction
        triggerAVNode = true;
        continuousMonitor.PlaySound(patient.Spo2);

    }

    // the second portion of the heart beat
    // QRS wave corresponds to ventricle contraction
    // should last about 0.1 seconds
    public IEnumerator QRSWave()			
    {
        float i = 0.0f;
        float start = Time.time;
        float complexTimer;

        // used to generate a value for the ST segment wait
        // this is basically a fancy way of getting some number between
        // 0.08 and 0.12
        complexTimer = Mathf.Clamp(0.04f + 0.08f * intervalR, 0.08f, 0.12f);

        // move from 0 to -1 quickly
        // time = 1 / 25 = 0.04 seconds
        while (i < 1.0f)
        {
            i += Time.deltaTime * Qrswavrate;
            voltageQRS = Mathf.SmoothStep(0.0f, -1.0f, i);
            yield return new WaitForFixedUpdate();
        }

        i = 0.0f;

        // here we use lerp to draw a straighter line
        // smoothstep would cause a rounded top of this wave which
        // would not be accurate
        // time = 1 / 25 = 0.04 -- this is not always accurate with lerp, lerp tends to move
        // faster sometimes
        while (i < 1.0f)
        {
            i += Time.deltaTime * Qrswavrate;
            voltageQRS = Mathf.Lerp(-1.0f, 8.0f, i);
            yield return new WaitForFixedUpdate();
        }
        // set the max voltage
        maxVolt = voltageQRS * voltageMultiplier;

        // start RR interval timer if this is the first beat
        if (firstBeat)
        {
            firstBeat = false;
            previousR = Time.time;
            currentR = Time.time;
        }
        // calculate how long the last beat took if we have completed a heart beat
        else
        {
            previousR = currentR;
            currentR = Time.time;
            // last time value - current time calue gives us an interval
            intervalR = currentR - previousR;
            StartCoroutine(RInterval());
        }
        i = 0.0f;
        // lerp back down to -2
        // time = 1/25 = 0.04
        while (i < 1.0f)
        {
            i += Time.deltaTime * Qrswavrate;
            voltageQRS = Mathf.Lerp(8.0f, -2.0f, i);
            yield return new WaitForFixedUpdate();
        }

        // start J wave thread while we handle the ST segment here
        // only adds a J wave if the add J wave variable is set
        StartCoroutine(JWave());

        i = 0.0f;

        // need to jump back to 0 quickly so create a new rate
        // stored in a new variable since other threads may need to access
        // the base qrs rate, prevents race condition
        float newQrsrate = Qrswavrate * 4;



        // lerp to 0
        // time 1 / 100 = 0.01
        while (i < 1.0f)
        {
            i += Time.deltaTime * newQrsrate;
            voltageQRS = Mathf.Lerp(-2.0f, 0.0f, i);
            yield return new WaitForFixedUpdate();
        }
        // QRS time at this point is around 0.13

        // the ST segment connects QRS to T wave
        // ST segment 0.08 to 0.12 s
        yield return new WaitForSeconds(segmentST);      
        // that should put QRS time at about 0.25
        // meaning we are at about 0.5 seconds total time

        // calculate time of this wave
        Qrstime = Time.time - start;
        // start the t wave
        StartCoroutine(TWave());
    }

    /*
     * J wave or Osborn wave can occur immediately following the QRS wave, they are not normal
     * but can be added if desired
     */
    public IEnumerator JWave()
    {
        float i = 0.0f;     

        float tempCoreSqrd;

        tempCoreSqrd = addJWave * addJWave;

        while (i < 1.0f)
        {
            i += Time.deltaTime * Jwavrate;
            voltageOsborn = Mathf.SmoothStep(0.0f, 6.0f * tempCoreSqrd, i);
            yield return new WaitForFixedUpdate();
        }

        i = 0.0f;


        while (i < 1.0f)
        {
            i += Time.deltaTime * Jwavrate;
            voltageOsborn = Mathf.SmoothStep(6.0f * tempCoreSqrd, 0.0f, i);
            yield return new WaitForFixedUpdate();
        }
    }

    // Duration = 0.4 seconds - ventricular repolarisation
    public IEnumerator TWave()			
    {
        float i = 0.0f;
        float start = Time.time;
        // time = 1/ 4.9 = about 0.2 seconds
        while (i < 1.0f)
        {
            i += Time.deltaTime * Twavrate;
            voltageT = Mathf.SmoothStep(0.0f, 2.0f, i);
            yield return new WaitForFixedUpdate();
        }

        i = 0.0f;
        // again about 0.2 seconds
        while (i < 1.0f)
        {
            i += Time.deltaTime * Twavrate;
            voltageT = Mathf.SmoothStep(2.0f, 0.0f, i);
            yield return new WaitForFixedUpdate();
        }
        Ttime = Time.time - start;

        // our total time at this point is about 0.9 seconds
        StartCoroutine(UWave());
    }

    // U wave should take about 0.08 seconds
    public IEnumerator UWave()
    {
        float i = 0.0f;
        float start = Time.time;
        // time = 1 / 25 = 0.04 s
        while (i < 1.0f)
        {
            i += Time.deltaTime * Uwavrate;
            voltageU = Mathf.SmoothStep(0.0f, 2.0f - invertUWave, i);
            yield return new WaitForFixedUpdate();
        }

        i = 0.0f;

        // again about 0.04 seconds
        while (i < 1.0f)
        {
            i += Time.deltaTime * Uwavrate;
            voltageU = Mathf.SmoothStep(2.0f - invertUWave, 0.0f, i);
            yield return new WaitForFixedUpdate();
        }
        Utime = Time.time - start;
        /* so now we are at about 0.98 seconds IN THEORY, there will obviously
        * be some deviation in that since the PR and ST segments can occur at different lengths
        * but with the rates I have, this gives us a default heart rate of about 60 bpm
        */
        triggerSANode = true;
    }

    public IEnumerator LeadsOff()
    {
        stateAsystole = true;
        StopAllCoroutines();
        yield return new WaitForFixedUpdate();
    }

    public IEnumerator MoveHeartRateTowards(float targetHR)
    {
        // P wave takes about 20% of the heart rate
        // I did this by taking default heart rate / old p wave value = div factor
        // 60 / 12.5 = 4.8 for default heart rate 
        // so for new rate take  targetHR / 4.8
        float pwavstart = Pwavrate;
        float pwavtarget = targetHR / 4.8f;

        // PR segment between PR and QRS takes about 10% of heart rate
        // take the reciprical since we need an actual time
        PRseglen = 1f / targetHR * 0.1f;

        // QRS also takes about 10%
        // 60 / 25 = 2.4
        // so new rate = target HR / 2.4
        float qrsstart = Qrswavrate;           
        float qrstarget = targetHR / 2.4f;

        // T wave takes about 40 %
        // default - 60 / 4.9 = 12.2
        // new - targetHR / 12.2
        float tstart = Twavrate;
        float ttarget = targetHR / 12.2f;

        // Uwave is the same as QRS wave, so we'll use those variables
        // J wave is already so fast that changing it would be negligible

        float i = 0;
        //float rate = 0.5f;  // increase/decrease the heart rate over 2 seconds
        float rate = 1/ (float)duration;
        while (i < 1.0f)
        {
            i += rate * Time.deltaTime;
            Pwavrate = Mathf.SmoothStep(pwavstart, pwavtarget, i);
            Qrswavrate = Mathf.SmoothStep(qrsstart, qrstarget, i);
            Twavrate = Mathf.SmoothStep(tstart, ttarget, i);
            Uwavrate = Mathf.SmoothStep(qrsstart, qrstarget, i);

            yield return new WaitForFixedUpdate();
        }
    }

    // return the heart rate to 60
    public override IEnumerator Stabilize()
    {

        // stop the thread execution so that we can reset all the flags without
        // causing a P wave and QRS wave to start at the same time
        // or something like that
        // StopAllCoroutines();
        stateAsystole = false;
        arhythmia = false;
        patient.arhythmia = false;
        eventOccuring = false;

        // set the rates for the waves



        Pwavrate = 12.5f;
        Qrswavrate = 25.0f;
        Jwavrate = 25.0f;
        Twavrate = 4.6f;
        Uwavrate = 25.0f;

        yield return new WaitForFixedUpdate();
    }

    // TODO: Make this work
    public IEnumerator Arhythmia()
    {
        bool positive = false;
        while (arhythmia)
        {
            float rateadjust;
            if (positive)
            {
                rateadjust = (0.5f + Random.value);
                positive = false;
            }
            else
            {
                rateadjust = (-0.5f - Random.value);
                positive = true;
            }

            Debug.Log("adjusting rates by" + rateadjust.ToString());
            Pwavrate += rateadjust;
            Twavrate += rateadjust;
            Uwavrate += rateadjust;

            yield return new WaitForFixedUpdate();
        }
        
    }
}
