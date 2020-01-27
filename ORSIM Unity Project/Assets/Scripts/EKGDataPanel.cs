using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * EKG Data Panel
 * Authors: Julian Dixon
 * Last Update: July 21 2016
 * 
 * This class handles the data displayed next to the EKGs in the patient monitor;
 */
public class EKGDataPanel : MonoBehaviour {

    private Text heartRateText;
    private Text stiitext;
    private Text stvtext;

    private Patient patient;

	// Use this for initialization
	void Start ()
    {
        // get the text objects from the scene
        heartRateText = GameObject.FindGameObjectWithTag("HR").GetComponent<Text>();
        stiitext = GameObject.FindGameObjectWithTag("stiidata").GetComponent<Text>();
        stvtext = GameObject.FindGameObjectWithTag("stvdata").GetComponent<Text>();

        // and the patient singleton
        patient = Patient.Instance;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //heartRateText.text = Mathf.RoundToInt(patient.HeartRate).ToString();
        stiitext.text = patient.STiivolt.ToString();
        stvtext.text = patient.STvvolt.ToString();
	}
}
