using UnityEngine;
using System.Collections;

/**
 * Original Author: Julian Dixon
 * Last Modified: April 19 2016
 * 
 * This script provides a graph manager base class that can be 
 * extended to suit the needs of other types of graphs
 * 
 * 
 * 
 */
public class GraphManager : MonoBehaviour {
    // values to give to MeshGraph script
    public float currentSum;
    public float previousSum;
    // values to be displayed in data panel
    public float maxVolt;

    public Color color; // set in editor

    // indicates if an event is occuring
    public bool eventOccuring;

    void Start()
    {
        currentSum = 0;
        previousSum = 0;
    }
    // basic sinasoid wave in the base graph, subclasses will have their own waveforms
    void FixedUpdate()
    {
        previousSum = currentSum;
        currentSum = Mathf.Sin(2* Mathf.PI * Time.timeSinceLevelLoad);
    }

    // simply prints the event name to the debug log, subclasses will implement this
    public virtual void HandleTimeEvent(TimeEvent te)
    {
        Debug.Log(te.evtType + te.factor.ToString() + te.startTime.ToString());
    }

    // only wait for fixed update here, subclasses should implement this
    public virtual IEnumerator Stabilize()
    {
        yield return new WaitForFixedUpdate();
    }
}
