using UnityEngine;
using System.Collections;
/*
 * RespManager
 * Authors: Julian Dixon
 * Last Update: July 12 2016
 * 
 * This class is no longer used, since we found that a respiratory graph
 * is rarely used in a patient monitor, since the end tidal CO2 graph
 * usually provides a good estimate of breathing rate Nonetheless,
 * feel free to add it back in if needed
 * 
 * Class to represent the breathing rate, and volume of air in the lungs
 */

public class RespManager : GraphManager {

    // variables used to calculate breath intervals
    private float brate;        // breathing rate per minute
    private float btime;          // breath time

    // variables for rate of inspiration, rate of expiration
    private float insprate;
    private float exprate;
    // the volume in our graph ( the y value )
    private float volume;
    // the volume scaled to the actual volume in the lungs
    private float scaledVolume;

    // flag for apnea occuring
    private bool apnea;
    // thread guards
    private bool startInspire;
    private bool startExpire;

    public AlarmManager alarmManager;

    // only events for this graph are apnea and stabilize
    public override void HandleTimeEvent(TimeEvent te)
    {
        eventOccuring = true;
        string type = te.evtType;

        if (type == "apnea")
        {
            apnea = true;
        }
        else if (type == "stabilize")
        {
            StartCoroutine(Stabilize());
        }
    }

    // set thread guard flags and get the alarm manager
    void Awake()
    {
        apnea = false;
        eventOccuring = false;
        startInspire = true;
        alarmManager = GameObject.FindGameObjectWithTag("AlarmObj").GetComponent<AlarmManager>();
    }

	// Use this for initialization
	void Start () {
        // set the starting values
        volume = 0.0f;
        brate = 12.0f;
        btime = 5.0f;
        insprate = 0.5f;
        exprate = 0.45f;
        currentSum = volume;        
	}

    void Update()
    {
        // don't start any new threads if we are in apnea
        if (apnea)
        {
            
        }
        else
        {
            // if we need to start inspiration (breathing in)
            if (startInspire)
            {
                StartCoroutine(Inspire());
                startInspire = false;
            }
            // or start expiration (breathing out)
            else if (startExpire)
            {
                StartCoroutine(Expire());
                startExpire = false;
            }
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        previousSum = currentSum;
        // apnea means we neither breathe in or out, so the volume stays constant
        // only update the current sum if we are not in apnea
        if (!apnea)
            currentSum = volume;

        // breathing rate (breaths per minute) = 60 seconds / seconds per breath
        brate = 60.0f / btime;

        // calculate actual volume in lungs
        if (volume <= 0.0f)
            scaledVolume = 2400.0f - Mathf.Abs(volume * 62.5f);
        else
            scaledVolume = 2400.0f + (volume * 62.5f);

	}

    public IEnumerator Inspire()
    {
        // move volume from 0 to 8 at insprate units per second
        float i = 0.0f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * insprate;
            volume = Mathf.SmoothStep(0.0f, 8.0f, i);
            yield return new WaitForFixedUpdate();
        }

        startExpire = true;
    }

    public IEnumerator Expire()
    {
        // same as inspire except from 8 to 0
        float i = 0.0f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * exprate;
            volume = Mathf.SmoothStep(8.0f, 0.0f, i);
            yield return new WaitForFixedUpdate();
        }

        // there is a natural pause between every breath, wait for 0.5 seconds
        // to simulate this
        yield return new WaitForSeconds(0.5f);

        startInspire = true;
    }

    // while apnea is set, simply wait for fixed update without changing any values
    public IEnumerator Apnea()
    {   
        while (apnea)
            yield return new WaitForFixedUpdate();
    }

    // restore defaults and stop the alarm
    public override IEnumerator Stabilize()
    {
        apnea = false;
        eventOccuring = false;
        alarmManager.StopAlarm();
        yield return new WaitForFixedUpdate();
    }
}
