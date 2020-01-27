using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* Skin Temp Manager
 * Authors: Julian Dixon
 * Last Update: July 21 2016
 * 
 * This class handles the skin temperature display in the patient monitor
 * the actual changing of the skin is done in the patient class
 */

public class SkinTempManager : MonoBehaviour {

    // set in editor
    public Text SkinTempText;

    private Patient patient;

	// Use this for initialization
	void Start ()
    {
        patient = Patient.Instance;
	}
	
	// Update is called once per frame
	void Update ()
    {
      //  SkinTempText.text = patient.Tskin.ToString();
	}
}
