using UnityEngine;
using System.Collections;

/*
 * Water Cm Graph Manager
 * Authors: Julian Dixon
 * Last Update: July 20 2016
 * 
 * This class handles the drawing of the paw cmh2o graph displayed at the top of the anesthesia
 * monitor
 */

public class WaterCmGraphManager : GraphManager {

    // variables we'll use to draw our graph
    private float pPeak;
    private float pMean;
    private float peep;

    private float phaseirate;
    private float phaseiirate;
    private float phaseiiirate;
    private float phaseivrate;

    // should usually be 1
    private float baseLine;

    // track what phase we are in
    private int phase;

    // place holder for currentSum
    private float cmh2o;

    private Patient patient;

	// Use this for initialization
	void Start ()
    {
        patient = Patient.Instance;

        pPeak = patient.Ppeak;
        pMean = patient.Pmean;
        peep = patient.Peep;

        phaseirate = 2f;
        phaseiirate = 1f;
        phaseiiirate = 0.5f;
        phaseivrate = 2f;
         
        baseLine = 1f;

        phase = 1;
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
        currentSum = cmh2o;
    }

    public IEnumerator PhaseI()
    {
        phase = 0;

        float i = 0f;
        // phaseirate = 2 so this should take 0.5 seconds
        // to jump from baseLine to pMean
        while (i < 1.0f)
        {
            i += Time.deltaTime * phaseirate;
            cmh2o = Mathf.Lerp(baseLine, pMean, i);
            yield return new WaitForFixedUpdate();
        }

        phase = 2;
    }

    public IEnumerator PhaseII()
    {
        phase = 0;

        float i = 0f;
        // phase 2 rate is 1 so this should take one second
        // to jump from pMean to pPeak - 2
        while (i < 1.0f)
        {
            i += Time.deltaTime * phaseiirate;
            cmh2o = Mathf.SmoothStep(pMean, pPeak - 2f, i);
            yield return new WaitForFixedUpdate();
        }

        // need to be quick with the "point" at the top of the graph
        i = 0f;
        // should take 1 / 15 seconds for this
        float tempRate = 15.0f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * tempRate;
            cmh2o = Mathf.Lerp(pPeak - 2f, pPeak, i);
            yield return new WaitForFixedUpdate();
        }

        phase = 3;
    }

    public IEnumerator PhaseIII()
    {
        phase = 0;

        float i = 0f;
        // phase 3 rate is 0.5 so this will take 2 seconds
        while (i < 1.0f)
        {
            i += Time.deltaTime * phaseiiirate;
            cmh2o = Mathf.SmoothStep(pPeak, peep, i);
            yield return new WaitForFixedUpdate();
        }

        phase = 4;
    }

    public IEnumerator PhaseIV()
    {
        phase = 0;

        float i = 0f;
        // phase 4 rate is 2 so this will also take 0.5 seconds
        while (i < 1.0f)
        {
            i += Time.deltaTime * phaseivrate;
            cmh2o = Mathf.SmoothStep(peep, baseLine + 1f, i);
            yield return new WaitForFixedUpdate();
        }

        // quickly lerp down to the baseline to create the "point" at the bottom
        i = 0f;
        float tempRate = 25f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * tempRate;
            cmh2o = Mathf.Lerp(baseLine + 1f, baseLine, i);
            yield return new WaitForFixedUpdate();
        }

        phase = 1;
    }
}
