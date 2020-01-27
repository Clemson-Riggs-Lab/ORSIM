using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using System.IO;

/*
 * Authors: Julian Dixon
 * Last Updated: July 12 2016
 * 
 * 
 * 
 * 
 *  Class:
 *      Singleton class to store information about the "patient"
 *      and related data obtained from the graphs
 *      Reads default values from the patientconfig.txt file
 */

public sealed class Patient : MonoBehaviour
{

    // basic information
    public string patientName;
    public int age;
    public bool gender;                 // true = female     false = male
    public string height;
    public string weight;
    public string inputFilePath;
    public DateTime startTimeReal;
    public GameObject VATHandler;


    // patient monitor graphs
    public GraphManager ecgstii;
    public GraphManager ecgstv;
    public GraphManager pulseox;

    // anesthesia graphs
    public GraphManager pawcmH2O;
    public GraphManager flow;
    public GraphManager etCO2;

    // raw text of events to be triggered (set in editor)
    public TextAsset txtAsset;
    public string TxtAsset;
    // list of events
    public ArrayList timedEventList { get; private set; }
    public TimeEvent currentEvent { get; private set; }
    public TimeEvent previousEvent { get; private set; }

    // start time of next event
    private float nextEventTime;
    // actual Time.time value when event starts
    private float currentEventStart = 0;
    // the time that the simulation begins
    private float startTime;

    // singleton stuff
    private static Patient _instance;
    //private Patient() { }

    public static Patient Instance
    {
        get
        {
            return _instance;
        }
    }

    // Initializer
    void Awake()
    {
        // if we have a different patient object than we should...
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);


        //set starting time of the simulation from cmd argument
        //creating a timespan variable
        DateTime s;
        // first we try to parse the starttimereal argument
        if (DateTime.TryParse(GetArg("-startTimeReal"), out s))
        {
            startTimeReal = s;
        }
        else
        {
            startTimeReal = DateTime.Now;

        }
        // end set starting time

        //set input file from command line 
        //checking that there is a inputfilepath argument
        if (!String.IsNullOrEmpty(GetArg("-inputFilePath")))
        {
            TxtAsset = File.ReadAllText(GetArg("-inputFilePath"));

            //TxtAsset = File.ReadAllText(@"C:\Users\aalami\Desktop\patientTest.txt");
            //TxtAsset = File.ReadAllText(GetArg("-inputFilePath"));
            //setting txt asset to the file we loaded
            //txtAsset = Resources.Load(GetArg("-inputFilePath")) as TextAsset;
        }
        else
        {
            TxtAsset = txtAsset.text;
        }
        //otherwise, the txtasset is whatever is in the resources.(the default)
        //end setting input file 




        // default values

        LoadPatientInfo();
        LoadAnesthesiaInfo();
        LoadEvents();
        LoadRanges();
    }

    void Start()
    {



        // be sure all of our graph managers have been initialized
        if (ecgstii == null)
        {
            ecgstii = GameObject.FindGameObjectWithTag("ST-II").GetComponent<GraphManager>();
        }
        if (ecgstv == null)
        {
            ecgstv = GameObject.FindGameObjectWithTag("ST-V").GetComponent<GraphManager>();
        }
        if (pulseox == null)
        {
            pulseox = GameObject.FindGameObjectWithTag("PulseOx").GetComponent<GraphManager>();
        }
        if (pawcmH2O == null)
        {
            pawcmH2O = GameObject.FindGameObjectWithTag("pawcmh2o").GetComponent<GraphManager>();
        }
        if (flow == null)
        {
            flow = GameObject.FindGameObjectWithTag("flowgraph").GetComponent<GraphManager>();
        }
        if (etCO2 = null)
        {
            etCO2 = GameObject.FindGameObjectWithTag("endtidalgraph").GetComponent<GraphManager>();
        }
    }

    void Update()
    {

        // if we have events still in the queue
        if (timedEventList.Count > 0)
        {
            // get the first one
            currentEvent = timedEventList[0] as TimeEvent;
            // if we need to trigger the event
            if (DateTime.Now >= currentEvent.startTime)
            {
                currentEventStart = Time.time;
                this.gameObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), currentEvent.evtCat + " " + currentEvent.evtType + " started", currentEvent.factor.ToString());
                // determine what graphs we need to send the trigger to
                if (currentEvent.evtCat == "HR")
                {
                    if (currentEvent.evtType == "change")
                    {
                        VATHandler.GetComponent<ValuesAndTextHandler>().HRUpdate(heartRate, currentEvent.factor, currentEvent.endTime, currentEvent);
                    }
                    else if (currentEvent.evtType == "stabilize")
                    {
                        VATHandler.GetComponent<ValuesAndTextHandler>().HRUpdate(heartRate, baseHeartRate, currentEvent.endTime, currentEvent);
                    }
                    ecgstii.HandleTimeEvent(currentEvent);
                    ecgstv.HandleTimeEvent(currentEvent);
                    pulseox.HandleTimeEvent(currentEvent);

                }
                else if (currentEvent.evtCat == "DBP")
                {

                    if (currentEvent.evtType == "change")
                    {
                        VATHandler.GetComponent<ValuesAndTextHandler>().DBPUpdate(diastolicPressure, currentEvent.factor, currentEvent.endTime, currentEvent);
                    }
                    else // stabilize D blood pressure,
                    {
                        VATHandler.GetComponent<ValuesAndTextHandler>().DBPUpdate(diastolicPressure, baseDiastolicPressure, currentEvent.endTime, currentEvent);
                    }
                }
                else if (currentEvent.evtCat == "SBP")
                {

                    if (currentEvent.evtType == "change")
                    {
                        VATHandler.GetComponent<ValuesAndTextHandler>().SBPUpdate(systolicPressure, currentEvent.factor, currentEvent.endTime, currentEvent);
                    }
                    else // stabilize S blood pressure,
                    {
                        VATHandler.GetComponent<ValuesAndTextHandler>().SBPUpdate(systolicPressure, baseSystolicPressure, currentEvent.endTime, currentEvent);
                    }
                }
                else if (currentEvent.evtCat == "SPO2")
                {
                    if (currentEvent.evtType == "change")
                    {
                        VATHandler.GetComponent<ValuesAndTextHandler>().SPO2Update(spo2, currentEvent.factor, currentEvent.endTime, currentEvent);
                    }
                    else if (currentEvent.evtType == "stabilize")
                    {
                        VATHandler.GetComponent<ValuesAndTextHandler>().SPO2Update(spo2, baseSpo2, currentEvent.endTime, currentEvent);
                    }
                    pulseox.HandleTimeEvent(currentEvent);
                }
                else if (currentEvent.evtCat == "ETCO2")
                {
                    if (currentEvent.evtType == "change")
                    {
                        VATHandler.GetComponent<ValuesAndTextHandler>().ETCO2Update(endTidalPeak, currentEvent.factor, currentEvent.endTime, currentEvent);
                    }
                }
                previousEvent = currentEvent;
                // remove the first item from the queue
                timedEventList.RemoveAt(0);
            }
        }
        // now handle the alarms
        HandleAlarms();
    }

    // parsing the basic patient information from the text file
    private void LoadPatientInfo()
    {
        //string rawtxt = txtAsset.text;
        string rawtxt = TxtAsset;
        string[] rawsplit = Regex.Split(rawtxt, "\r\n");

        if (string.Compare(rawsplit[0], "~") != 0)
        {
            Debug.Log("Error reading config file 1");
        }

        if (string.Compare(rawsplit[1], "Patient Monitor Config") != 0)
        {
            Debug.Log("Error reading config file 2");
        }

        for (int i = 2; rawsplit[i] != "~"; i++)
        {
            string[] colonSplit = rawsplit[i].Split(':');
            if (colonSplit[0] == "Name")
            {
                patientName = colonSplit[1];
            }
            else if (colonSplit[0] == "Gender")
            {
                if (colonSplit[1] == "Male")
                {
                    gender = false;
                }
                else if (colonSplit[1] == "Female")
                {
                    gender = true;
                }
            }
            else if (colonSplit[0] == "Age")
            {
                age = int.Parse(colonSplit[1]);
            }
            else if (colonSplit[0] == "Height")
            {
                height = colonSplit[1];
            }
            else if (colonSplit[0] == "Weight")
            {
                weight = colonSplit[1];
            }
            else if (colonSplit[0] == "HeartRate" || colonSplit[0] == "Pulse")
            {
                heartRate = float.Parse(colonSplit[1]);
                baseHeartRate = heartRate;

                pulse = heartRate;
                basePulse = pulse;
            }
            else if (colonSplit[0] == "SPO2")
            {
                spo2 = float.Parse(colonSplit[1]);
                baseSpo2 = spo2;
            }
            else if (colonSplit[0] == "Systolic")
            {
                systolicPressure = float.Parse(colonSplit[1]);
                baseSystolicPressure = systolicPressure;
            }
            else if (colonSplit[0] == "Diastolic")
            {
                diastolicPressure = float.Parse(colonSplit[1]);
                baseDiastolicPressure = diastolicPressure;
            }
            else if (colonSplit[0] == "Tskin")
            {
                skinTemp = float.Parse(colonSplit[1]);
                baseSkinTemp = skinTemp;
            }
            else if (colonSplit[0] == "ETCO2")
            {
                EtPeak = float.Parse(colonSplit[1]);
                baseETCO2 = EtPeak;
            }
        }
    }

    // load anesthesia information, TODO
    private void LoadAnesthesiaInfo()
    {
        pPeak = 13f;
        pMean = 8f;
        peep = 5f;
        tvExpmL = 535f;
        mV = 9.1f;
        mVspont = 9.1f;
        fival = 1f;
        endTidalRR = 14f;
        complcmH2O = 101f;
        o2et = 45f;
        o2fi = 51f;
        isoet = 1.1f;
        isofi = 1.3f;
        mac = 1.1f;
        o2percentage = 60f;
        totalflow = 2f;
        ventmode = "PSVPro";
        pinsp = 12f;
        ventrr = 10f;
        flowcntrl = 2.0f;
        prsupp = 5f;
        if (endTidalPeak == 0f)
        {
            endTidalPeak = 43f;
        }
    }

    // load time events
    private void LoadEvents()
    {

        // initialize arraylist
        timedEventList = new ArrayList();

        /*
        timedEventList.Add(new TimeEvent("HR increase(35.0 00:30)"));
        timedEventList.Add(new TimeEvent("HR stabilize(0.0 00:45)"));
        timedEventList.Add(new TimeEvent("BP increase(10.0 00:55)"));
        timedEventList.Add(new TimeEvent("SPO2 decrease(9.0 01:25)"));
        timedEventList.Add(new TimeEvent("SPO2 stabilize(0.0 01:35)"));
        timedEventList.Add(new TimeEvent("HR arhythmia(0.0 01:45)"));
        timedEventList.Add(new TimeEvent("HR stabilize(0.0 01:55)"));
        */

        //string rawEvents = txtAsset.text;
        string rawEvents = TxtAsset;
        string[] lines = Regex.Split(rawEvents, "\r\n");

        int i;
        for (i = 0; lines[i] != "Patient Monitor Events"; i++)
        {
            // this loop just puts i at the right position
        }
        i++;

        for (string eventLine; i < lines.Length && lines[i] != "~"; i++)
        {
            //Debug.Log(eventLine);
            eventLine = lines[i];
            timedEventList.Add(new TimeEvent(eventLine));
        }

        timedEventList.Sort(new TimeEvent.TimeEventComparer());
    }


    private void LoadRanges()
    {

        string rawEvents = TxtAsset;
        string[] lines = Regex.Split(rawEvents, "\r\n");

        int i;
        for (i = 0; lines[i] != "Values Ranges"; i++)
        {
            // this loop just puts i at the right position
        }
        i++;

        for (string rangeLine; i < lines.Length && lines[i] != "~"; i++)
        {
            //Debug.Log(eventLine);
            rangeLine = lines[i];
            VATHandler.GetComponent<ValuesAndTextHandler>().AddRange(rangeLine);
        }
    }



    /*
     * VARIABLES TO HOLD DATA FROM GRAPHS IN PATIENT MONITOR
     */

    // circulatory system related data - configured by user in setup
    // heart rate -- from ECG
    private float heartRate;
    public float HeartRate
    {
        get
        {
            return heartRate;
        }
        set
        {
            heartRate = value;
        }
    }

    // flag for cardiac arhythmia NOT IMPLEMENTED YET
    public bool arhythmia;

    // max voltages for both EKGs
    private float stiimaxvoltage;
    public float STiivolt
    {
        get
        {
            GraphManager gm = GameObject.FindGameObjectWithTag("ST-II").GetComponent<GraphManager>();
            return gm.maxVolt / 8.0f;
        }
    }
    private float stvmaxvoltage;
    public float STvvolt
    {
        get
        {
            GraphManager gm = GameObject.FindGameObjectWithTag("ST-V").GetComponent<GraphManager>();
            return gm.maxVolt / 8.0f;
        }
    }

    // SPO2 -- or the percentage of saturated hemoglobin in the blood
    // from pulse ox graph
    private float spo2;
    public float Spo2
    {
        get
        {
            return spo2;
        }
        set
        {
            spo2 = value;
        }
    }

    // pulse -- should be roughly equal to heart rate
    private float pulse;
    public float Pulse
    {
        get
        {
            return pulse;
        }
        set
        {
            pulse = value;
        }
    }

    // blood pressure variables -- no graphs for blood pressure
    // since Non-Invasive blood pressure is displayed only as text
    private float systolicPressure;
    public float SystolicPress
    {
        get
        {
            return systolicPressure;
        }
        set
        {
            systolicPressure = value;
        }
    }
    private float diastolicPressure;
    public float DiastolicPress
    {
        get
        {
            return diastolicPressure;
        }
        set
        {
            diastolicPressure = value;
        }
    }

    // data to be calculated at run time since Mean Arterial Pressure is calculated from other values
    // displayed in parenthisis next to blood pressure in patient monitor
    private float meanArterialPressure;
    public float MeanAP
    {
        get
        {
            // generalized estimation formula from wikipedia
            meanArterialPressure = diastolicPressure + 0.01f * Mathf.Exp(4.14f - 40.74f / heartRate) * (systolicPressure - diastolicPressure);
            return meanArterialPressure;
        }
        set
        {
            meanArterialPressure = value;
        }
    }

    // skin temperature data
    private float skinTemp;
    public float Tskin
    {
        get
        {
            return skinTemp;
        }
        set
        {
            skinTemp = value;
        }
    }

    // respiratory data -- NO LONGER DISPLAYED IN PATIENT MONITOR
    // we eliminated the use of the respiratory graph, this is left here for legacy pruposes
    private float breathRate;
    public float BreathRate
    {
        get
        {
            return breathRate;
        }
        set
        {
            breathRate = value;
        }
    }
    /*
     * END OF PATIENT MONITOR VARIABLES
     */

    /* 
     * VARIABLES TO HOLD DATA FROM ANESTHESIA MONITOR 
     */
    // paw cmH2O data
    private float pPeak;
    public float Ppeak
    {
        get
        {
            return Mathf.Round(pPeak);
        }
        set
        {
            pPeak = value;
        }
    }
    private float pMean;
    public float Pmean
    {
        get
        {
            return Mathf.Round(pMean);
        }
        set
        {
            pMean = value;
        }
    }

    private float peep;
    public float Peep
    {
        get
        {
            return Mathf.Round(peep);
        }
        set
        {
            peep = value;
        }
    }

    // flow data
    private float mV;
    public float Mv
    {
        get
        {
            return mV;
        }
        set
        {
            mV = value;
        }
    }

    private float mVspont;
    public float MvSpont
    {
        get
        {
            return mVspont;
        }
        set
        {
            mVspont = value;
        }
    }

    private float tvExpmL;
    public float TVexpml
    {
        get
        {
            return Mathf.Round(tvExpmL);
        }
        set
        {
            tvExpmL = value;
        }
    }

    // co2 mmHg panel data
    private float endTidalPeak;
    public float EtPeak
    {
        get
        {
            return Mathf.Round(endTidalPeak);
        }
        set
        {
            endTidalPeak = value;
        }
    }

    private float fival;
    public float FiVal
    {
        get
        {
            return Mathf.Round(fival);
        }
        set
        {
            fival = value;
        }
    }

    // data for resp panel
    // end tidal CO2 interval rate, should be equal to breaths per minute
    private float endTidalRR;
    public float EtRR
    {
        get
        {
            return Mathf.Round(endTidalRR);
        }
        set
        {
            endTidalRR = value;
        }
    }

    private float complcmH2O;
    public float ComplCmH2o
    {
        get
        {
            return Mathf.Round(complcmH2O);
        }
        set
        {
            complcmH2O = value;
        }
    }

    // data from Gases % panel
    private float o2et;
    public float O2Et
    {
        get
        {
            return Mathf.Round(o2et);
        }
        set
        {
            o2et = value;
        }
    }

    private float o2fi;
    public float O2Fi
    {
        get
        {
            return Mathf.Round(o2fi);
        }
        set
        {
            o2fi = value;
        }
    }

    // data from Agent % panel
    private float isoet;
    public float IsoEt
    {
        get
        {
            return isoet;
        }
        set
        {
            isoet = value;
        }
    }

    private float isofi;
    public float IsoFi
    {
        get
        {
            return isofi;
        }
        set
        {
            isofi = value;
        }
    }

    private float mac;
    public float Mac
    {
        get
        {
            return mac;
        }
        set
        {
            mac = value;
        }
    }

    // data from fresh gas panel
    private float o2percentage;
    public float O2percent
    {
        get
        {
            return Mathf.Round(o2percentage);
        }
        set
        {
            o2percentage = value;
        }
    }

    private float totalflow;
    public float TotalFlow
    {
        get
        {
            return Mathf.Round(totalflow);
        }
        set
        {
            totalflow = value;
        }
    }

    // data from ventilator panel
    private string ventmode;
    public string VentMode
    {
        get
        {
            return ventmode;
        }
        set
        {
            ventmode = value;
        }
    }

    private float pinsp;
    public float Pinsp
    {
        get
        {
            return Mathf.Round(pinsp);
        }
        set
        {
            pinsp = value;
        }
    }

    private float ventrr;
    public float VentRR
    {
        get
        {
            return Mathf.Round(ventrr);
        }
        set
        {
            ventrr = value;
        }
    }

    private float flowcntrl;
    public float FlowControl
    {
        get
        {
            return Mathf.Round(flowcntrl);
        }
        set
        {
            flowcntrl = value;
        }
    }

    private float prsupp;
    public float PRsupport
    {
        get
        {
            return Mathf.Round(prsupp);
        }
        set
        {
            prsupp = value;
        }
    }
    // these are the base values of the variables. they are extracted from the input file
    public float baseHeartRate { get; private set; }
    public float basePulse { get; private set; }
    public float baseSpo2 { get; private set; }
    public float baseSystolicPressure { get; private set; }
    public float baseDiastolicPressure { get; private set; }
    public float baseSkinTemp { get; private set; }
    public float baseETCO2 { get; private set; }

    // PEEP also displayed on the panel, grab from the paw cmh2o data

    /*
     * END OF ANESTHESIA MONITOR VARIABLES
     */



    // alarms and management functions
    private AlarmManager alarmManager;

    // checks the values obtained from the graphs to see if we need to raise an alarm
    private void HandleAlarms()
    {
        alarmManager = GameObject.FindGameObjectWithTag("AlarmObj").GetComponent<AlarmManager>();

        if (alarmManager.playing == false)
        {
            if (spo2 < 90f)
            {
                alarmManager.StartAlarm((int)EventTypes.SPO2LOW);
            }
            /// the alarms below were commented by jawad on the request of kylie
            /// They were commented because simulating the operating room, alarms are only present for SPO2 decrease and HR change.
            /// HR alarms were also commented because kylie said that we are not presenting HR change cues, and HR changes with SPO2 in 
            /// our scenarios, so they would trigger together, and thus there isnt a need for the HR alarm

            //else if (heartRate >= 90f && heartRate <= 110f)
            //{
            //    alarmManager.StartAlarm((int)EventTypes.HRHIGH);
            //}
            //else if (heartRate > 110f)
            //{
            //    alarmManager.StartAlarm((int)EventTypes.XTACHY);
            //}
            //else if (heartRate <= 50f && heartRate > 30f)
            //{
            //    alarmManager.StartAlarm((int)EventTypes.HRLOW);
            //}
            //else if (heartRate <= 30f)
            //{
            //    alarmManager.StartAlarm((int)EventTypes.XBRADY);
            //}
            //else if (heartRate == 0f)
            //{
            //    alarmManager.StartAlarm((int)EventTypes.ASYSTOLE);
            //}
            //else if (arhythmia)
            //{
            //    alarmManager.StartAlarm((int)EventTypes.ARHYTHMIA);
            //}
            //else if (meanArterialPressure < 60f)
            //{
            //    alarmManager.StartAlarm((int)EventTypes.BPLOW);
            //}
            //else if (meanArterialPressure > 110f)
            //{
            //    alarmManager.StartAlarm((int)EventTypes.BPHIGH);
            //}

        }
        else
        {
            if (spo2 >= 90f)
            {
                alarmManager.StopAlarm();
            }
            /// the alarms below were commented by jawad on the request of kylie
            /// They were commented because simulating the operating room, alarms are only present for SPO2 decrease and HR change.
            /// HR alarms were also commented because kylie said that we are not presenting HR change cues, and HR changes with SPO2 in 
            /// our scenarios, so they would trigger together, and thus there isnt a need for the HR alarm

            //if (heartRate >= 60f && heartRate <= 90f && spo2 >= 90f && meanArterialPressure >= 60f && meanArterialPressure <= 110f && !arhythmia)
            //{
            //    alarmManager.StopAlarm();
            //}
        }

    }

    // no graph for blood pressure, so increasing and decreasing values is handled here
    private void ChangeDBloodPressure(float f)
    {
        diastolicPressure = f;
    }

    private void ChangeSBloodPressure(float f)
    {
        systolicPressure = f;
    }
    // log reaction time into debug console
    public void LogReactionTime()
    {
        float reactionTime = Time.time - currentEventStart;
        Debug.Log("Reaction to " + previousEvent.evtCat + " " + previousEvent.evtType + " was " + reactionTime.ToString());
    }

    // Helper function for getting the command line arguments'
    // this is used to get the file path and the start time
    // below is an example
    // Eg:  C:\Program Files\Unity\Unity.exe -outputDir "c:\temp\output" ...
    // read the "-outputDir" command line argument
    //var outputDir = GetArg("-outputDir");

    private static string GetArg(string name)
    {
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }
}
