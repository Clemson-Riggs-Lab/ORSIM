using UnityEngine;
using System.Collections;

/*
 * VolGraphLine class
 * Authors: Julian Dixon
 * Last Update: July 12 2016
 * 
 * Class to draw the vol/paw line in the anesthesia monitor
 * the mesh graph was not suitable for this because this graph has
 * to loop backwards. So I used a particle line
 */
public class VolGraphLine : ParticleLine {

    // variable for volume (our y value) and cmh2o (our x value)
    private float volume;
    private float cmh2o;

    // flags to serve as locks for threads
    private bool calculatingVolume;
    private bool calculatingcmh2o;
    private bool updatingPoints;

    public GraphManager cmh2oManager;

	// Use this for initialization
	void Start ()
    {
        calculatingVolume = false;
        calculatingcmh2o = false;
        updatingPoints = false;
        volume = 1f; // start volume at high point
        cmh2o = 0;

        // get the particle system and create the initial points
        // CreatePoints defined in superclass
        partSys = GetComponent<ParticleSystem>();
        CreatePoints();
	}
	
	// Update is called once per frame
    // regular update handles the thread logic similarly to the Mesh Graphs
	void Update ()
    {
	    if (!calculatingVolume)
        {
            StartCoroutine(VolumeCalculator());
        }
        if (!calculatingcmh2o)
        {
            StartCoroutine(Cmh2oCalculator());
        }
        if (!updatingPoints)
        {
            StartCoroutine(UpdatePoints());
        }
	}

    // use fixed update to set the particles
    void FixedUpdate()
    {
        partSys.SetParticles(points, points.Length);
    }

    // loop over the points and set the positions
    protected override IEnumerator UpdatePoints()
    {
        // thread guard
        updatingPoints = true;
        //loop over points
        for (int i = 0; i < points.Length; i++)
        {
            // set new position
            points[i].position = new Vector3(cmh2o, volume);
            // wait for fixed update, so that we can get a new cmh2o and volume value
            // and update one point at a time
            yield return new WaitForFixedUpdate();
        }
        updatingPoints = false;
    }

    // calculate the volume (our y value)
    // NOTE: this is probably not how the volume works, still doing research
    // on how this line is drawn
    // TODO: make this more accurate
    public IEnumerator VolumeCalculator()
    {
        // thread guard
        calculatingVolume = true;


        float i = 0;
        float rate = 0.5f;
        // move volume from 1 to 0 at given rate
        while (i < 1.0f)
        {
            i += rate * Time.deltaTime;
            volume = Mathf.SmoothStep(1f, 0f, i);
            // wait for fixed update to set new particle position
            yield return new WaitForFixedUpdate();
        }

        i = 0;
        rate = 0.5f;
        // do the same as above, except from 0 to 1 now
        while (i < 1.0f)
        {
            i += rate * Time.deltaTime;
            volume = Mathf.SmoothStep(0f, 1f, i);
            yield return new WaitForFixedUpdate();
        }
        // unguard thread
        calculatingVolume = false;
    }

    // calculate the cmh2o (our x value)
    public IEnumerator Cmh2oCalculator()
    {
        // thread guard
        calculatingcmh2o = true;
        // get the current sum from our graph
        // divided by 35 hear to keep the graph small
        // I know its messy, sorry
        cmh2o = (cmh2oManager.currentSum / 35f);
        yield return new WaitForFixedUpdate();
        calculatingcmh2o = false;
    }
}
