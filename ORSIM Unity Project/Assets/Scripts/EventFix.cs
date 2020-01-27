using UnityEngine;
using System.Collections;

/**
 * Original Author: Julian Dixon
 * Last Edited: April 19 2016
 * 
 * This was made entirely for our demonstration on April 19, it is a general
 * "fix whatever problem is happening" class that nullifies any event that 
 * is triggering an alarm
 * 
 */
public class EventFix : MonoBehaviour {

    // list of all graphs, and graph that has an event happening
    public GraphManager[] graphs;
    public GraphManager currentEvent;
    Patient patient;

	// Use this for initialization
	void Start () {

        // list of gameobjects with graph components
        GameObject[] graphObjs = GameObject.FindGameObjectsWithTag("graph");
        int graphCount = graphObjs.Length;
        graphs = new GraphManager[graphCount];

        // get the graphmanager objects from the gameobjects
        int i;
        for (i = 0; i < graphCount; i++)
            graphs[i] = graphObjs[i].GetComponentInChildren<GraphManager>();

        patient = Patient.Instance;
	}

    public void FixEvent()
    {
        // call the virtualized Stabilize function, should return any graph
        // to its default state
        foreach (GraphManager eventGraph in graphs)
        {
            if (eventGraph.eventOccuring)
            {
                currentEvent = eventGraph;
                StartCoroutine(currentEvent.Stabilize());
            }
        }
        
        // get the reaction time for it's event
        patient.LogReactionTime();
    }
}
