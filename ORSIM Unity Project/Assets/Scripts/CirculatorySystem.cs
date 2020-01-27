using UnityEngine;
using System.Collections;

/*
 * Circulcatory System
 * Authors: Julian Dixon
 * Last Update: July 14 2016
 * 
 * This idea never came into light, the idea was to hold variables needed to draw the graphs
 * in here to try to make synchronizing the pulse and heart rate graph easier.
 * 
 * Feel free to run with this idea but its going to involve a lot of code refactoring
 */
public class CirculatorySystem : MonoBehaviour {

    /* define some default values */
    public static float HEART_RATE_DEFAULT = 60.0f; // also serves as default pulse rate
    public static float SYSTOLIC_PRESSURE_DEFAULT = 120.0f;
    public static float DIASTOLIC_PRESSURE_DEFAULT = 80.0f;
    public static float SPO2_DEFAULT = 96.0f;
    public static float LOW_HR_THRESH = 45.0f;
    public static float HIGH_HR_THRESH = 90.0f;
    public static float HIGH_SYSTOLIC_THRESH = 130.0f;
    public static float HIGH_DIASTOLIC_THRESH = 100.0f;

    /* variables relating to EKG */
    private bool triggerSA;                         // stimulate the atria
    private bool triggerAV;                         // stimulate the ventricles

    private float interval;                         // time of one heart beat
    private float[] intervalArray;
    private float heartRate;                        // calculated every 10 intervals

    public float ecgVoltage;                        // voltage at any given point
    public float previousVoltage;

    // variables controlling the different segments
    private float pwavePeak;
    private float pwaveRate;

    private float qrssegMin;
    private float qrssegPeak;
    private float qrssegRate;

    private float stSegmentTime;
    private float twavePeak;
    private float twaveRate;

    private float uwavePeak;
    private float uwaveRate;

    // variables for EKG alarms
    private bool lowHR;
    private bool highHR;
    private bool arhythmia;
    private bool vTach; // ventricular tachycardia
    private bool brady; // extreme low HR
    private bool asystole; // flatline

    private float varianceHR;

    /* Non-Invasive Blood Pressure related variables */
    private bool systole;
    private bool diastole;

    private float measureInterval;                      // how often we measure blood pressure
    private float lastMeasure;                          // time we took last measure

    public float systolicPressure;
    public float diastolicPressure;
    public float aorticNotchPressure;

    /* pulse oximetry related */
    public float oxysaturation;                         // SpO2


    public AlarmManager alarmManager;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
