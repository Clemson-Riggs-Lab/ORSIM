using UnityEngine;
using System.Collections;
/*
 * Particle Line class
 * Authors: Julian Dixon
 * Last Update: July 12 2016
 * 
 * Class draws a line with a particle system, rather than a mesh
 * Originally tried to create the ECG traces with this, but it remained useful
 * for creating dotted lines, and drawing the volume/paw graph in the anesthesia
 * monitor
 */

public class ParticleLine : MonoBehaviour {
    // variables to set in editor
    public Color lineColor;
    [Range(10, 200)]
    public int resolution = 10;
    public int lineWidth;
    public float particleSize;

    // protected variables
    protected int currentResolution;
    protected ParticleSystem.Particle[] points;

    protected ParticleSystem partSys;

    // Initializer
    void Start()
    {
        // get our particle system from the game objects components
        partSys = GetComponent<ParticleSystem>();
        // create the initial points
        CreatePoints();
    }

    protected void CreatePoints()
    {
        // get our current resolution and initialize array of particles
        currentResolution = resolution;
        points = new ParticleSystem.Particle[resolution];

        // get an increment for how far apart to spread the particles
        float increment = (float) lineWidth / (resolution - 1);

        // loop over particle array
        for (int i = 0; i < resolution; i++)
        {
            // get the x position, index * increment
            float x = i * increment;
            // set the particle's position, color, and size
            points[i].position = new Vector3(x, 0f, 0f);
            points[i].startColor = lineColor;
            points[i].startSize = particleSize;
        }
    }

    // in base class, keep the line straight, subclasses can override this
    void Update()
    {
        partSys.SetParticles(points, points.Length);
    }

    // doesn't do anything here, subclasses provide implementation
    protected virtual IEnumerator UpdatePoints()
    {
        yield return new WaitForFixedUpdate();
    }
}
