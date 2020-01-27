using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/*
 * Continuos Monitor
 * Authors: Julian Dixon
 * Last Update: July 14 2016
 * 
 * This class handles the continuous monitoring, which is the beeps you hear at each
 * heart beat tick. This is separated from the Alarm Manager because it should only
 * stop sounding when an alarm is activated
 */
public class ContinousMonitor : MonoBehaviour {

    // private variables
    private AudioSource audioSource;
    private Text dataText;                      // text in top right where pulse is displayed
    private Patient patient;

    // make sure this object doesn't get destroyed when activating the 2nd display
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start ()
    {
        // get the audiosource, text object, and patient singleton
        audioSource = GetComponent<AudioSource>();
        dataText = GameObject.FindGameObjectWithTag("MonitorData").GetComponent<Text>();
        patient = Patient.Instance;
	}
	
    // just needs to update the text displayed
    void Update ()
    {
        //dataText.text = Mathf.RoundToInt(patient.Pulse).ToString();
    }

    // calculate our pitch and play the sound
    // the pitch changes according to how saturated the blood is
    // lower blood saturation = higher frequency
    // WORK IN PROGRESS
    public void PlaySound(float spo2)
    {
        // audioSource.pitch = 1.5f + ((float)Math.Round(spo2 / 5.0)* 5  * -0.005f);
         audioSource.pitch = 1.5f + ((float)Math.Round(100 / 5.0)* 5  * -0.005f);

        audioSource.Play();
    } 

    // mute and unmute functions for when we need to play an alarm
    public void Mute()
    {
        audioSource.mute = true;
    }

    public void Unmute()
    {
        audioSource.mute = false;
    }
}
