using UnityEngine;
using System.Collections;

/*
 * Flow Graph Manager
 * Authors: Julian Dixon
 * Last Update: July 20 2016
 * 
 * This class handles the flow graph in the middle part of the anesthesia display
 */

public class FlowGraphManager : GraphManager {

    private float lpm;          // liters per minute
    private float graphRate;    // rate the trace moves

    private float graphPeak;    // max height the graph reaches
    private float inflect;      // inflection point of graph
    private float graphMin;     // lowest point graph touches

    private int phase;          // track the current phase we are in

	// Use this for initialization
	void Start () {
        lpm = 0f;
        phase = 3;

        graphPeak = 12f;
        graphMin = -12f;
        inflect = -1.5f;
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
        currentSum = lpm;
    }

    public IEnumerator PhaseI()
    {
        phase = 0;
        float i = 0f;

        // rate for 0.8 seconds
        graphRate = 1.25f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * graphRate;
            lpm = Mathf.SmoothStep(0f, graphPeak, i);
            yield return new WaitForFixedUpdate();
        }

        phase = 2;
    }

    public IEnumerator PhaseII()
    {
        phase = 0;
        float i = 0f;

        // rate for 0.4 seconds
        graphRate = 2.25f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * graphRate;
            lpm = Mathf.SmoothStep(graphPeak, graphMin, i);
            yield return new WaitForFixedUpdate();
        }
        phase = 3;
    }

    public IEnumerator PhaseIII()
    {
        phase = 0;
        float i = 0f;
        graphRate = 0.55f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * graphRate;
            lpm = Mathf.SmoothStep(graphMin, inflect, i);
            yield return new WaitForFixedUpdate();
        }
        phase = 4;
    }

    public IEnumerator PhaseIV()
    {
        phase = 0;
        float i = 0f;

        // rate for 1 second
        graphRate = 1f;
        while (i < 1.0f)
        {
            i += Time.deltaTime * graphRate;
            lpm = Mathf.SmoothStep(inflect, 0f, i);
            yield return new WaitForFixedUpdate();
        }
        phase = 1;
    }
}
