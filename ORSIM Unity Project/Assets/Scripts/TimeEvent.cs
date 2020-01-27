using UnityEngine;
using System.Collections;
using System;


/**
 * Original Author: Julian Dixon
 * Last Modified: April 19 2016
 * 
 * This script provides an interface for events that occur at a specific
 * time mark, theres also a Comparer class
 * 
 * 
 * 
 */
public class TimeEvent
{

    // comparer class for timeevents
    /*
     * TimeEventComparer
     * implements the IComparer interface for our TimeEvent Class
     */
    public class TimeEventComparer : IComparer
    {
        // returns -1 if the x occurs before y
        //          0 if they start at the same time
        //          1 if x occurs after y
        int IComparer.Compare(object x, object y)
        {
            TimeEvent tx = x as TimeEvent;
            TimeEvent ty = y as TimeEvent;


            return tx.startTime.CompareTo(ty.startTime);
        }
    }

    private string evt;     // raw string read from text file
    public string evtCat;   // catagory of event (heart rate, blood pressure, etc)
    public string evtType;  // type of event (increase, decrease, etc.)
    public float factor;       // value used to make events happen
    public DateTime startTime; // time the event begins
    public DateTime endTime; // time the event ends
    private Patient patient;


    /**
     * constructor parses the string from the config file
     * format for events is:
     * 
     * event_catagory event_type (<arg> <timestamp>)
     * the spacing is important
     * timestamp should be in a MM:SS format, indicating how long after
     * the simulation starts the event should happen
     * arg will be a number treated as a float
     */

    public TimeEvent(string s)
    {
        patient = Patient.Instance;
        evt = s;
        // split the string into tokens
        string[] evtSplit = evt.Split(' ', '(', ';', ')');

        // save category and type
        evtCat = evtSplit[0];
        evtType = evtSplit[1];
        // need fload.parse to get a number from this token
        factor = float.Parse(evtSplit[3]);

        // parse out time, need to split the last taken of the original split
        startTime = patient.startTimeReal.Add(TimeSpan.Parse(evtSplit[4]));

        //end time
        endTime = patient.startTimeReal.Add(TimeSpan.Parse(evtSplit[5]));
    }
}
