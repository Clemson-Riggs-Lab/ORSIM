using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
/*
* Pleth Data Panel Manager
* Authors: Julian Dixon
* Last Update: July 21 2016
* 
* This class handles the display of the data produced by the pleth graph
* in the patient monitor
*/
public class PlethDataPanelManager : MonoBehaviour {

    public Text spo2data;

    private Patient patient;
	// Use this for initialization
	void Start () {
        patient = Patient.Instance;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (patient.previousEvent != null && DateTime.Now < patient.previousEvent.endTime)
        {
                
                //spo2data.text = (patient.previousEvent.endTime- patient.previousEvent.startTime);
        }
        else
        {
            //spo2data.text = patient.Spo2.ToString();
        }
    }
}
