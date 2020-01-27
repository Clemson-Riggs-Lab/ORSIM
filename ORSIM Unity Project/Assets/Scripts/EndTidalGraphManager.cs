using UnityEngine;
using System.Collections;

/*
 * End Tidal Graph Manager
 * Authors: Julian Dixon
 * 
 * This class handles how the EtCO2 graph is drawn
 */

public class EndTidalGraphManager : GraphManager {

    private float phaseitime;           // inspiratory baseline     phase i
    private float phaseiirate;          // expiratory upstroke      phase ii
    private float phaseiiirate;         // alveolar plateau         phase iii
    private float phaseivrate;          // inspiratory downstroke   phase iv

    private int phase;                  // keep track of which phase we are in

    private bool firstbreath;           // used to measure the interval time
    private float lastInterval;
    private float currentInterval;

    private float[] etrrarray;
    private float breathrate;

    // the peak of the graph, should be around 40
    private float maxco2;
    // the value we'll use as a placeholder for current sum
    private float co2press;

    // our end tidal value scaled to give to the patient for display
    private float scaledEt;

    private Patient patient;

	// Use this for initialization
	void Start ()
    {
        patient = Patient.Instance;

        phaseitime = 0.8f;       // 0.8 seconds
        phaseiirate = 2f;        // 0.5 seconds
        phaseiiirate = 0.5f;     // 2 seconds
        phaseivrate = 1.6f;      // 0.625 seconds

        /*
         * total time per breath is 3.925 seconds
         * so ( 60 seconds / 1 minute ) * ( 1 breath / 4 seconds ) = 15 breaths per minute
         * which is a normal rate
         */
        
        // get our starting value for where our graph should peak
        scaledEt = patient.EtPeak;

        // scale it down to our graphs size
        maxco2 = scaledEt / 4.3f;

        co2press = 0f;

        // set phase to 1 so we can start our breathing
        phase = 1;

        firstbreath = true;
        etrrarray = new float[10];
        for (int i = 0; i < 10; i++)
        {
            etrrarray[i] = 4f;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (phase == 1)
        {
            StartCoroutine(PhaseI());
        }
        else if (phase == 2)
        {
            StartCoroutine(PhaseII());
        }
        else if (phase == 3)
        {
            StartCoroutine(PhaseIII());
        }
        else if (phase == 4)
        {
            StartCoroutine(PhaseIV());
        }
	}

    void FixedUpdate()
    {
        previousSum = currentSum;
        currentSum = co2press;

        //patient.EtPeak = scaledEt;
    }

    // phase i, sit at our baseline for the time value we have in our
    // private variable
    public IEnumerator PhaseI()
    {
        phase = 0;
        co2press = 0;
        yield return new WaitForSeconds(phaseitime);
        phase=2;
    }

    public IEnumerator PhaseII()
    {
        phase = 0;
        float i = 0.0f;

        // move toward maxco2 - 2 over 0.5s
        while (i < 1.0f)
        {
            i += Time.deltaTime * phaseiirate;
            co2press = Mathf.SmoothStep(0, maxco2 - 2, i);
            yield return new WaitForFixedUpdate();
        }
        phase=3;
    }

    public IEnumerator PhaseIII()
    {
        phase = 0;
        float i = 0.0f;
        // this phase is slow, so we make the "plateau" over 2 seconds
        while (i < 1.0f)
        {
            i += Time.deltaTime * phaseiiirate;
            co2press = Mathf.SmoothStep(maxco2-2, maxco2, i);
            yield return new WaitForFixedUpdate();
        }

        if (firstbreath)
        {
            lastInterval = Time.time;
            currentInterval = Time.time;
            firstbreath = false;
        }
        else
        {
            lastInterval = currentInterval;
            currentInterval = Time.time;
            StartCoroutine(EtInterval(currentInterval - lastInterval));
        }
        phase=4;
    }

    public IEnumerator PhaseIV()
    {
        phase = 0;
        float i = 0.0f;
        // move back to the baseline over 0.625 seconds
        while (i < 1.0f)
        {
            i += Time.deltaTime * phaseivrate;
            co2press = Mathf.SmoothStep(maxco2, 0, i);
            yield return new WaitForFixedUpdate();
        }
        phase = 1;
    }

    public IEnumerator EtInterval(float etTime)
    {
        float average = 0f;
        for (int i = 0; i < 9; i++)
        {
            etrrarray[i] = etrrarray[i + 1];
            average += etrrarray[i];
        }

        etrrarray[9] = etTime;
        average += etTime;
        average /= 10;

        breathrate = (1f / average) * 60f;
        patient.BreathRate = breathrate;
        patient.EtRR = breathrate;
        yield return new WaitForFixedUpdate();
    }
}
