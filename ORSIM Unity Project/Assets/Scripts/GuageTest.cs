using UnityEngine;
using System.Collections;

/*
 * Just a class to test if my guage was working
 */
public class GuageTest : GraphManager {

    private bool done = true;
	// Use this for initialization
	void Start () {
        currentSum = 0;
	}
	
    void Update()
    {
        if (done)
        {
            StartCoroutine(ZeroToTwenty());
            done = false;
        }
    }
	// Update is called once per frame
	void FixedUpdate () {
        previousSum = currentSum;
	}

    public IEnumerator ZeroToTwenty()
    {
        float i = 0;
        float rate = 1.5f;

        while (i < 1.0f)
        {
            i += rate * Time.deltaTime;
            currentSum = Mathf.SmoothStep(0.0f, 20.0f, i);
            yield return new WaitForFixedUpdate();
        }
        StartCoroutine(TwentyToZero());
    }

    public IEnumerator TwentyToZero()
    {
        float i = 0;
        float rate = 0.5f;

        while (i < 1.0f)
        {
            i += rate * Time.deltaTime;
            currentSum = Mathf.SmoothStep(20.0f, 0.0f, i);
            yield return new WaitForFixedUpdate();
        }
        done = true;
    }
}
