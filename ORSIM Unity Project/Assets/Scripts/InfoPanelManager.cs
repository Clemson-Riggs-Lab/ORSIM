using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Globalization;

/* Info Panel Manager
 * Authors: Julian Dixon
 * Last Update: July 21 2016
 * 
 * This class handles the information displayed on the panel at the top of the patient monitor
 */

public class InfoPanelManager : MonoBehaviour {

    Patient patient;

    // it would probably be easier to make these public and set them in the editor
    // than it would to use the tags that i've made so that I can get them
    // but it works like this so not a pressing issue
    private Image gender;
    private Text pname;
    private Text age;
    private Text height;
    private Text weight;
    private Text date;
    private Text start;
    private Text curr;

	// Use this for initialization
	void Start () {

        patient = Patient.Instance;


        pname = GameObject.FindGameObjectWithTag("Name").GetComponent<Text>();
        pname.text = patient.patientName;

        age = GameObject.FindGameObjectWithTag("Age").GetComponent<Text>();
        age.text = patient.age.ToString();

        height = GameObject.FindGameObjectWithTag("Height").GetComponent<Text>();
        height.text = patient.height;

        weight = GameObject.FindGameObjectWithTag("Weight").GetComponent<Text>();
        weight.text = patient.weight;

        gender = GameObject.FindGameObjectWithTag("Gender").GetComponent<Image>();
        if (patient.gender)
        {
            gender.sprite = Resources.Load<Sprite>("Images/female-symbol-pink");
        }
        else
        {
            gender.sprite = Resources.Load<Sprite>("Images/Male_Blue");
        }

        date = GameObject.FindGameObjectWithTag("Date").GetComponent<Text>();
        date.text = DateTime.Now.ToShortDateString();

        start = GameObject.FindGameObjectWithTag("StartTime").GetComponent<Text>();
        start.text = DateTime.Now.ToLongTimeString();

        curr = GameObject.FindGameObjectWithTag("RunTime").GetComponent<Text>();
        curr.text = DateTime.Now.ToLongTimeString();
    }
	
	// Update is called once per frame
	void Update () {
        date.text = DateTime.Now.ToLongDateString();
        curr.text = DateTime.Now.ToLongTimeString();
	}
}
