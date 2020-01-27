using UnityEngine;
using System.Collections;

/*
 * Pleth Manager
 * Authors: Julian Dixon
 * Last Update: July 21 2016
 * 
 * This class handles the drawing of the plethysmogram near the bottom of the
 * patient monitor
 */
public class PlethManager : GraphManager
{

    Patient patient;

    // thread guarding flags
    private bool diastole;
    private bool systole;
    private bool between;

    // the target saturation we need to get
    public float targetSaturation;
    // target / 10
    public float scaledTarget;

    // base of the graph
    private float baseLine;

    // rates for the systolic and diastolic phases, and how long to wait between
    private float srate;
    private float drate;
    private float betweenTime;

    // the current saturation
    private float saturation;
    // how much we need to vary
    private float variance = 0.5f;

    // flag used for calculating intervals
    private bool firstPulse;
    // array and variables used for calculating intervals
    private float[] intervalArray;
    public float lastInterval;
    private float prevTime;
    private float currentTime;
    private float duration = 2;
    // the pulse we produce (should be the same as the heart rate
    public float pulse;

    // continuous monitoring (beeps) is done here
    private ContinousMonitor continuousMonitor;

    // set flags apropriately
    void Awake()
    {
        systole = false;
        diastole = false;
        between = false;
        firstPulse = true;
    }

    // Use this for initialization
    void Start()
    {
        continuousMonitor = GameObject.FindGameObjectWithTag("ContMonitor").GetComponent<ContinousMonitor>();
        patient = Patient.Instance;

        targetSaturation = patient.Spo2;
        scaledTarget = targetSaturation / 10.0f;

        pulse = patient.Pulse;

        baseLine = 1.0f;

        // one pulse should take the same time as the heart rate
        // the time between systole and diastole is about 54% of one 
        // beat, so (60 bpm * 0.54) / targetBPM = between seconds
        betweenTime = (60f * 0.545f) / pulse;

        // for a pulse of 60, srate = 5, drate = 3.4
        // so for any pulse, srate = pulse / 12, drate = pulse / 17.65
        srate = pulse / 9f;
        drate = pulse / 15.65f;

        currentSum = baseLine;
        previousSum = baseLine;

        intervalArray = new float[10];
        for (int i = 0; i < 10; i++)
        {
            intervalArray[i] = pulse / 60.0f;
        }

        between = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (between)
        {
            StartCoroutine(TimeBetween());
            between = false;
        }
        if (systole)
        {
            StartCoroutine(Systole());
            systole = false;
        }
        if (diastole)
        {
            StartCoroutine(Diastole());
            diastole = false;
        }

    }

    void FixedUpdate()
    {
        previousSum = currentSum;
        currentSum = saturation + (variance + (-0.5f + Random.value));
        //patient.Pulse = pulse;
        //patient.Spo2 = targetSaturation;
    }

    // handle a time based event
    public override void HandleTimeEvent(TimeEvent te)
    {
        duration = (int)(te.endTime - te.startTime).TotalSeconds;
        eventOccuring = true;
        if (te.evtCat == "HR")
        {
            if (te.evtType == "change")
            {
                StartCoroutine(MovePulseToward(te.factor));
            }
            else if (te.evtType == "stabilize")
            {
                StartCoroutine(MovePulseToward(patient.basePulse));
                // StartCoroutine(Stabilize());
            }
        }
        else if (te.evtCat == "SPO2")
        {
            if (te.evtType == "change")
            {

                targetSaturation = te.factor;
            }
            else if (te.evtType == "stabilize")
            {
                targetSaturation = patient.baseSpo2;
            }

            scaledTarget = targetSaturation / 10f;
        }
    }


    public IEnumerator PulseInterval()
    {
        float avgInterval = 0.0f;
        // move each interval down one in the array, and start calculating average
        for (int i = 0; i < 9; i++)
        {
            intervalArray[i] = intervalArray[i + 1];
            avgInterval += intervalArray[i];
        }
        // compute the most recent interval and add it to the array
        lastInterval = currentTime - prevTime;
        intervalArray[9] = lastInterval;

        // finish calculating average
        avgInterval += lastInterval;
        avgInterval /= 10.0f;

        // avg interval is "seconds per pulse" so we take the reciprical to get "pulse per second"
        float perSecondPulse = 1.0f / avgInterval;
        // convert it to per minute
        pulse = perSecondPulse * 60.0f;
        yield return new WaitForFixedUpdate();
    }

    // simply waits for the amount of seconds specified
    public IEnumerator TimeBetween()
    {
        yield return new WaitForSeconds(betweenTime);
        // with our default value of 0.54, we have gotten to 0.54 seconds total

        // lets Update() call systole
        systole = true;
    }

    public IEnumerator Systole()
    {
        float i = 0.0f;
        // with the default s rate of 5 this will move from the 
        // base line to the target in 1 / 5 seconds or 0.2 s
        while (i < 1.0f)
        {
            i += Time.deltaTime * srate;
            saturation = Mathf.SmoothStep(baseLine, scaledTarget, i);
            yield return new WaitForFixedUpdate();
        }
        // so with the default between time wait and default s rate
        // we should be at 0.54 s + 0.2 s = 0.74 seconds total

        // continuous monitor beeps at the peak right here
        //continuousMonitor.PlaySound(patient.Spo2);

        // handle calculating the pulse
        if (firstPulse)
        {
            prevTime = Time.time;
            currentTime = Time.time;
            firstPulse = false;
        }
        else
        {
            prevTime = currentTime;
            currentTime = Time.time;
            StartCoroutine(PulseInterval());
        }
        // allow Update() to call diastole thread
        diastole = true;

    }

    public IEnumerator Diastole()
    {
        float i = 0.0f;
        // with the default rate of 3.4, this will move from the target
        // back to our base line in 1 / 3.4 seconds, or about 0.29 s
        while (i < 1.0f)
        {
            i += Time.deltaTime * drate;
            saturation = Mathf.SmoothStep(scaledTarget, baseLine, i);
            yield return new WaitForFixedUpdate();
        }

        /* so with the between time at 0.54, srate at 5, and drate at 3.4, this puts us
         * at a base pulse time of 0.54 + 0.2 + 0.29 = 0.98 seconds, same as the EKG, 
         * the variation between the two should be about as minimized as possible at this point
         */
        between = true;
    }



    public IEnumerator MovePulseToward(float targetPulse)
    {
        // one pulse should take the same time as the heart rate
        // so in between systole and diastole is about 70% the heart beat time
        // 0.5 for 60 bpm = 1 beat / 1 second * 7 / 10
        // so between time = pulse / 60 s * 7 / 10
        float betweenStart = betweenTime;
        float betweenTimeTarget = (60f * 0.55f) / targetPulse;

        // for a pulse of 60, srate = 5, drate = 3.4
        // so for any pulse, srate = pulse / 12, drate = pulse / 17.65
        float sratestart = srate;
        float sratetarget = targetPulse / 9f;

        float dratestart = drate;
        float dratetarget = targetPulse / 15.65f;

        float i = 0.0f;
        float rate = 1 / duration;
        // move the rates and between time to our targets over 2 seconds
        while (i < 1.0f)
        {
            i += rate * Time.deltaTime;
            betweenTime = Mathf.SmoothStep(betweenStart, betweenTimeTarget, i);
            srate = Mathf.SmoothStep(sratestart, sratetarget, i);
            drate = Mathf.SmoothStep(dratestart, dratetarget, i);
            yield return new WaitForFixedUpdate();
        }
    }

    // return graph to default state
    public override IEnumerator Stabilize()
    {
        eventOccuring = false;

        srate = 6.25f;
        drate = 3.5f;
        betweenTime = 0.5f;

        yield return new WaitForFixedUpdate();
    }
}
