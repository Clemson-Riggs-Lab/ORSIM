using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/*
 * Blood Pressure Manager
 * Authors: Julian Dixon
 * Last Update: July 14 2016
 * 
 * This class controls the blood pressure panel displayed in the bottom
 * right corner of the 
 * 
 * 
 * 
 *  monitor
 * It mostly just handles the displaying of the text. The changing of blood pressure values is
 * handled in the Patient class.
 */
public class BloodPressureManager : MonoBehaviour {

    // text objects, set in editor
    public Text systolic;
    public Text diastolic;
    public Text meanartpress;
    public Text interval;
    public Text lastMeasure;

    // patient singleton
    private Patient patient;

    // the last measurement time stamp
    // we want to re--measure every 3 minutes
    public static DateTime lastMeasureTime;

	// Use this for initialization
	void Start ()
    {
        // get our patient singleton
        patient = Patient.Instance;
        // get our data from patient
        interval.text = "Auto 3:00";
        //systolic.text = patient.SystolicPress.ToString();
        //diastolic.text = patient.DiastolicPress.ToString();
        meanartpress.text = "(" + (Mathf.RoundToInt(patient.MeanAP)).ToString() + ")";
        // set the last measurement time variable and the text displaying the last measured time
        lastMeasureTime = DateTime.Now;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // check if we need to re-measure
        // 180.0f = 180 seconds = 3 minutes

            //update the text displayed if necessary
            //systolic.text = patient.SystolicPress.ToString();
            //diastolic.text = patient.DiastolicPress.ToString();
            meanartpress.text = "(" + (Mathf.RoundToInt(patient.MeanAP)).ToString() + ")";
            lastMeasure.text = lastMeasureTime.ToShortTimeString();
        
        
	}
}
