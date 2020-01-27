using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class ValuesAndTextHandler : MonoBehaviour
{
    System.Random randomNumberGenerator;
    Patient patient;
    private Text HRText;
    private Text SPO2Text;
    private Text BPSText;
    private Text BPDText;
    private Text PulseText;
    private Text ETCO2Text;

    private Text TSKINText;
    private Text pPeakText;
    private Text pMeanText;
    private Text peepText;
    private Text tvExpmLText;
    private Text mVspontText;
    private Text mVText;
    private Text fiText;
    private Text isoetText;
    private Text isofiText;
    private Text mac62yText;
    private Text o2etText;
    private Text o2fiText;
    private Text CO2RRminText;
    private Text complcmH2OText;
    private Text o2percentText;
    private Text totalflowText;
    private Text pinspText;
    private Text RRText;

    bool UpdateHR;
    bool UpdateSPO2;
    bool UpdateETCO2;
    bool UpdateDBP;
    bool UpdateSBP;

    DateTime HRUpdateStartTime;
    DateTime HRUpdateEndTime;
    DateTime DBPUpdateStartTime;
    DateTime DBPUpdateEndTime;
    DateTime SBPUpdateStartTime;
    DateTime SBPUpdateEndTime;
    DateTime SPO2UpdateStartTime;
    DateTime SPO2UpdateEndTime;
    DateTime ETCO2UpdateStartTime;
    DateTime ETCO2UpdateEndTime;

    DateTime NextBPReading;
    DateTime NextHRReading;
    DateTime NextSPO2Reading;
    DateTime NextETCO2Reading;

    DateTime NextTSKINReading;
    DateTime NextpPeakReading;
    DateTime NextpMeanReading;
    DateTime NextpeepReading;
    DateTime NexttvExpmLReading;
    DateTime NextmVspontReading;
    DateTime NextmVReading;
    DateTime NextfiReading;
    DateTime NextisoetReading;
    DateTime NextisofiReading;
    DateTime Nextmac62yReading;
    DateTime Nexto2etReading;
    DateTime Nexto2fiReading;
    DateTime NextCO2RRminReading;
    DateTime NextcomplcmH2OReading;
    DateTime Nexto2percentReading;
    DateTime NexttotalflowReading;
    DateTime NextpinspReading;
    DateTime NextRRReading;

    float HRInitial;
    float BPSInitial;
    float BPDInitial;
    float SPO2Initial;
    float ETCO2Initial;

    float HRFinal;
    float BPSFinal;
    float BPDFinal;
    float SPO2Final;
    float ETCO2Final;


    float pPeakFinal;
    float pMeanFinal;
    float peepFinal;
    float tvExpmLFinal;
    float mVspontFinal;
    float mVFinal;
    float fiFinal;
    float isoetFinal;
    float isofiFinal;
    float mac62yFinal;
    float o2etFinal;
    float o2fiFinal;
    float CO2RRminFinal;
    float complcmH2OFinal;
    float o2percentFinal;
    float totalflowFinal;
    float pinspFinal;
    float RRFinal;
    float TSKINFinal;


    float HRrate;
    float SPrate;
    float DPrate;
    float SPO2rate;
    float ETCO2rate;
    TimeEvent currentHREvent;
    TimeEvent currentDBPEvent;
    TimeEvent currentSBPEvent;
    TimeEvent currentSPO2Event;
    TimeEvent currentETCO2Event;
    GameObject PatientObject;


    float PulseRange = 0;
    float SPO2Range = 0;
    float BPRange = 0;
    float ETCO2Range = 0;
    float pPeakRange = 0;
    float pMeanRange = 0;
    float peepRange = 0;
    float tvExpmLRange = 0;
    float mVspontRange = 0;
    float mVRange = 0;
    float fiRange = 0;
    float isoetRange = 0;
    float isofiRange = 0;
    float mac62yRange = 0;
    float o2etRange = 0;
    float o2fiRange = 0;
    float CO2RRminRange = 0;
    float complcmH2ORange = 0;
    float o2percentRange = 0;
    float totalflowRange = 0;
    float pinspRange = 0;
    float RRRange = 0;
    float HRRange = 0;
    float TSKINRange = 0;


    float PulseUpdateResolution = 60;
    float SPO2UpdateResolution = 60;
    float BPUpdateResolution = 60;
    float ETCO2UpdateResolution = 60;
    float pPeakUpdateResolution = 60;
    float pMeanUpdateResolution = 60;
    float peepUpdateResolution = 60;
    float tvExpmLUpdateResolution = 60;
    float mVspontUpdateResolution = 60;
    float mVUpdateResolution = 60;
    float fiUpdateResolution = 60;
    float isoetUpdateResolution = 60;
    float isofiUpdateResolution = 60;
    float mac62yUpdateResolution = 60;
    float o2etUpdateResolution = 60;
    float o2fiUpdateResolution = 60;
    float CO2RRminUpdateResolution = 60;
    float complcmH2OUpdateResolution = 60;
    float o2percentUpdateResolution = 60;
    float totalflowUpdateResolution = 60;
    float pinspUpdateResolution = 60;
    float RRUpdateResolution = 60;
    float HRUpdateResolution = 60;
    float TSKINUpdateResolution = 60;



    float PulseUpdateResolutionVariationPercentage = 15;
    float SPO2UpdateResolutionVariationPercentage = 15;
    float BPUpdateResolutionVariationPercentage = 15;
    float ETCO2UpdateResolutionVariationPercentage = 15;
    float pPeakUpdateResolutionVariationPercentage = 15;
    float pMeanUpdateResolutionVariationPercentage = 15;
    float peepUpdateResolutionVariationPercentage = 15;
    float tvExpmLUpdateResolutionVariationPercentage = 15;
    float mVspontUpdateResolutionVariationPercentage = 15;
    float mVUpdateResolutionVariationPercentage = 15;
    float fiUpdateResolutionVariationPercentage = 15;
    float isoetUpdateResolutionVariationPercentage = 15;
    float isofiUpdateResolutionVariationPercentage = 15;
    float mac62yUpdateResolutionVariationPercentage = 15;
    float o2etUpdateResolutionVariationPercentage = 15;
    float o2fiUpdateResolutionVariationPercentage = 15;
    float CO2RRminUpdateResolutionVariationPercentage = 15;
    float complcmH2OUpdateResolutionVariationPercentage = 15;
    float o2percentUpdateResolutionVariationPercentage = 15;
    float totalflowUpdateResolutionVariationPercentage = 15;
    float pinspUpdateResolutionVariationPercentage = 15;
    float RRUpdateResolutionVariationPercentage = 15;
    float HRUpdateResolutionVariationPercentage = 15;
    float TSKINUpdateResolutionVariationPercentage = 15;


    // Use this for initialization
    void Start()
    {
        randomNumberGenerator = new System.Random();

        PatientObject = GameObject.FindGameObjectWithTag("Patient");
        patient = Patient.Instance;
        HRText = GameObject.FindGameObjectWithTag("HR").GetComponent<Text>();
        SPO2Text = GameObject.FindGameObjectWithTag("SPO2").GetComponent<Text>();
        BPSText = GameObject.FindGameObjectWithTag("BPS").GetComponent<Text>();
        BPDText = GameObject.FindGameObjectWithTag("BPD").GetComponent<Text>();
        ETCO2Text = GameObject.FindGameObjectWithTag("ETCO2").GetComponent<Text>();
        PulseText = GameObject.FindGameObjectWithTag("MonitorData").GetComponent<Text>();

        HRText.text = patient.HeartRate.ToString();
        BPSText.text = patient.SystolicPress.ToString();
        BPDText.text = Mathf.RoundToInt(patient.DiastolicPress).ToString();
        SPO2Text.text = Mathf.RoundToInt(patient.Spo2).ToString();
        PulseText.text = Mathf.RoundToInt(patient.Pulse).ToString();

        TSKINText = GameObject.FindGameObjectWithTag("TSKIN").GetComponent<Text>();
        pPeakText = GameObject.FindGameObjectWithTag("ppeakdata").GetComponent<Text>();
        pMeanText = GameObject.FindGameObjectWithTag("pMean").GetComponent<Text>();
        peepText = GameObject.FindGameObjectWithTag("peep").GetComponent<Text>();
        tvExpmLText = GameObject.FindGameObjectWithTag("tvExpmL").GetComponent<Text>();
        mVspontText = GameObject.FindGameObjectWithTag("mVspont").GetComponent<Text>();
        mVText = GameObject.FindGameObjectWithTag("mV").GetComponent<Text>();
        fiText = GameObject.FindGameObjectWithTag("fi").GetComponent<Text>();
        isoetText = GameObject.FindGameObjectWithTag("isoet").GetComponent<Text>();
        isofiText = GameObject.FindGameObjectWithTag("isofi").GetComponent<Text>();
        mac62yText = GameObject.FindGameObjectWithTag("mac62y").GetComponent<Text>();
        o2etText = GameObject.FindGameObjectWithTag("o2et").GetComponent<Text>();
        o2fiText = GameObject.FindGameObjectWithTag("o2fi").GetComponent<Text>();
        CO2RRminText = GameObject.FindGameObjectWithTag("CO2RRmin").GetComponent<Text>();
        complcmH2OText = GameObject.FindGameObjectWithTag("complcmH2O").GetComponent<Text>();
        o2percentText = GameObject.FindGameObjectWithTag("o2percent").GetComponent<Text>();
        totalflowText = GameObject.FindGameObjectWithTag("totalflow").GetComponent<Text>();
        pinspText = GameObject.FindGameObjectWithTag("pinsp").GetComponent<Text>();
        RRText = GameObject.FindGameObjectWithTag("RR").GetComponent<Text>();

        HRFinal = patient.HeartRate;
        SPO2Final = patient.Spo2;
        ETCO2Final = patient.EtPeak;
        TSKINFinal = patient.Tskin;
        BPDFinal = patient.DiastolicPress;
        BPSFinal = patient.SystolicPress;
        pPeakFinal = patient.Ppeak;
        pMeanFinal = patient.Pmean;
        peepFinal = patient.Peep;
        tvExpmLFinal = patient.TVexpml;
        mVspontFinal = patient.MvSpont;
        mVFinal = patient.Mv;
        fiFinal = patient.FiVal;
        isoetFinal = patient.IsoEt;
        isofiFinal = patient.IsoFi;
        mac62yFinal = patient.Mac;
        o2etFinal = patient.O2Et;
        o2fiFinal = patient.O2Fi;
        CO2RRminFinal = patient.EtRR;
        complcmH2OFinal = patient.ComplCmH2o;
        o2percentFinal = patient.O2percent;
        totalflowFinal = patient.TotalFlow;
        pinspFinal = patient.Pinsp;
        RRFinal = patient.VentRR;
    }
    public void HRUpdate(float currentHR, float FinalHR, DateTime EndTime, TimeEvent currentEvent)
    {
        HRUpdateStartTime = DateTime.Now;
        HRUpdateEndTime = EndTime;
        HRInitial = currentHR;
        HRFinal = FinalHR;
        HRrate = (FinalHR - currentHR) / (float)(EndTime - DateTime.Now).TotalMilliseconds;
        UpdateHR = true;
        currentHREvent = currentEvent;
    }
    public void DBPUpdate(float currentDpressure, float FinalDpressure, DateTime EndTime, TimeEvent currentEvent)
    {
        DBPUpdateStartTime = DateTime.Now;
        DBPUpdateEndTime = EndTime;
        BPDInitial = currentDpressure;
        BPDFinal = FinalDpressure;
        DPrate = (FinalDpressure - currentDpressure) / (float)(EndTime - DateTime.Now).TotalMilliseconds;
        UpdateDBP = true;
        currentDBPEvent = currentEvent;

    }
    public void SBPUpdate(float currentSpressure, float FinalSpressure, DateTime EndTime, TimeEvent currentEvent)
    {
        SBPUpdateStartTime = DateTime.Now;
        SBPUpdateEndTime = EndTime;
        BPSInitial = currentSpressure;
        BPSFinal = FinalSpressure;
        SPrate = (FinalSpressure - currentSpressure) / (float)(EndTime - DateTime.Now).TotalMilliseconds;
        UpdateSBP = true;
        currentSBPEvent = currentEvent;

    }
    public void SPO2Update(float currentSPO2, float FinalSPO2, DateTime EndTime, TimeEvent currentEvent)
    {
        SPO2UpdateStartTime = DateTime.Now;
        SPO2UpdateEndTime = EndTime;
        SPO2Initial = currentSPO2;
        SPO2Final = FinalSPO2;
        SPO2rate = (FinalSPO2 - currentSPO2) / (float)(EndTime - DateTime.Now).TotalMilliseconds;
        UpdateSPO2 = true;
        currentSPO2Event = currentEvent;

    }
    public void ETCO2Update(float currentETCO2, float FinalETCO2, DateTime EndTime, TimeEvent currentEvent)
    {
        ETCO2UpdateStartTime = DateTime.Now;
        ETCO2UpdateEndTime = EndTime;
        ETCO2Initial = currentETCO2;
        ETCO2Final = FinalETCO2;
        ETCO2rate = (FinalETCO2 - currentETCO2) / (float)(EndTime - DateTime.Now).TotalMilliseconds;
        UpdateETCO2 = true;
        currentETCO2Event = currentEvent;

    }
    // Update is called once per frame
    void Update()
    {


        if (UpdateHR)
        {
            if (DateTime.Now < HRUpdateEndTime && (patient.HeartRate != patient.Pulse || patient.HeartRate != HRFinal))
            {
                if (DateTime.Now > NextHRReading)
                {
                    patient.HeartRate = (HRrate * (float)(DateTime.Now - HRUpdateStartTime).TotalMilliseconds) + HRInitial;
                    patient.Pulse = patient.HeartRate;
                    HRText.text = Mathf.RoundToInt(patient.HeartRate).ToString();
                    PulseText.text = Mathf.RoundToInt(patient.Pulse).ToString();
                    NextHRReading = DateTime.Now.AddSeconds(HRUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * HRUpdateResolution * HRUpdateResolutionVariationPercentage / 100);
                    BloodPressureManager.lastMeasureTime = DateTime.Now;
                }
            }
            else
            {

                PatientObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), currentHREvent.evtCat + " " + currentHREvent.evtType + " Ended", currentHREvent.factor.ToString());
                patient.HeartRate = patient.Pulse = HRFinal;
                HRText.text = PulseText.text = Mathf.RoundToInt(HRFinal).ToString();
                PulseText.text = Mathf.RoundToInt(patient.Pulse).ToString();

                UpdateHR = false;
            }
        }

        else
        {
            if (DateTime.Now > NextHRReading)
            {
                patient.HeartRate = patient.Pulse = HRFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * HRRange;
                HRText.text = PulseText.text = Mathf.RoundToInt(patient.HeartRate).ToString();
                NextHRReading = DateTime.Now.AddSeconds(HRUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * HRUpdateResolution * HRUpdateResolutionVariationPercentage / 100);
            }
        }

        if (UpdateSBP)
        {

            if (DateTime.Now < SBPUpdateEndTime && patient.SystolicPress != BPSFinal)
            {
                if (DateTime.Now > NextBPReading)
                {
                    patient.SystolicPress = (SPrate * (float)(DateTime.Now - SBPUpdateStartTime).TotalMilliseconds) + BPSInitial;
                    BPSText.text = Mathf.RoundToInt(patient.SystolicPress).ToString();
                    NextBPReading = DateTime.Now.AddSeconds(BPUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * BPUpdateResolution * BPUpdateResolutionVariationPercentage / 100);
                }
            }
            else
            {
                PatientObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), currentSBPEvent.evtCat + " " + currentSBPEvent.evtType + " Ended", currentSBPEvent.factor.ToString());
                patient.SystolicPress = BPSFinal;
                BPSText.text = Mathf.RoundToInt(BPSFinal).ToString();
                UpdateSBP = false;
            }
        }
        else
        {
            if (DateTime.Now > NextBPReading)
            {
                patient.SystolicPress = BPSFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * BPRange;
                BPSText.text = Mathf.RoundToInt(patient.SystolicPress).ToString();
                NextBPReading = DateTime.Now.AddSeconds(BPUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * BPUpdateResolution * BPUpdateResolutionVariationPercentage / 100);
            }
        }
        if (UpdateDBP)
        {

            if (DateTime.Now < DBPUpdateEndTime && patient.DiastolicPress != BPDFinal)
            {
                if (DateTime.Now > NextBPReading)
                {
                    patient.DiastolicPress = (DPrate * (float)(DateTime.Now - DBPUpdateStartTime).TotalMilliseconds) + BPDInitial;
                    BPDText.text = Mathf.RoundToInt(patient.DiastolicPress).ToString();
                    NextBPReading = DateTime.Now.AddSeconds(BPUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * BPUpdateResolution * BPUpdateResolutionVariationPercentage / 100);
                }
            }
            else
            {
                PatientObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), currentDBPEvent.evtCat + " " + currentDBPEvent.evtType + " Ended", currentDBPEvent.factor.ToString());
                patient.DiastolicPress = BPDFinal;
                BPDText.text = Mathf.RoundToInt(BPDFinal).ToString();
                UpdateDBP = false;
            }
        }
        else
        {
            if (DateTime.Now > NextBPReading)
            {
                patient.DiastolicPress = BPDFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * BPRange;
                BPDText.text = Mathf.RoundToInt(patient.DiastolicPress).ToString();
                NextBPReading = DateTime.Now.AddSeconds(BPUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * BPUpdateResolution * BPUpdateResolutionVariationPercentage / 100);
            }
        }
        if (UpdateSPO2)
        {
            if (DateTime.Now < SPO2UpdateEndTime)
            {
                if (DateTime.Now > NextSPO2Reading && patient.Spo2 != SPO2Final)
                {
                    patient.Spo2 = (SPO2rate * (float)(DateTime.Now - SPO2UpdateStartTime).TotalMilliseconds) + SPO2Initial;
                    SPO2Text.text = Mathf.RoundToInt(patient.Spo2).ToString();
                    NextSPO2Reading = DateTime.Now.AddSeconds(SPO2UpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * SPO2UpdateResolution * SPO2UpdateResolutionVariationPercentage / 100);
                    //patient.pulseox.HandleTimeEvent(patient.previousEvent);


                }
            }
            else
            {
                PatientObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), currentSPO2Event.evtCat + " " + currentSPO2Event.evtType + " Ended", currentSPO2Event.factor.ToString());
                patient.Spo2 = SPO2Final;
                SPO2Text.text = Mathf.RoundToInt(SPO2Final).ToString();
                UpdateSPO2 = false;
            }
        }
        else
        {
            if (DateTime.Now > NextSPO2Reading)
            {
                patient.Spo2 = SPO2Final + (float)(randomNumberGenerator.NextDouble() - 0.5) * SPO2Range;
                SPO2Text.text = Mathf.RoundToInt(patient.Spo2).ToString();
                NextSPO2Reading = DateTime.Now.AddSeconds(SPO2UpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * SPO2UpdateResolution * SPO2UpdateResolutionVariationPercentage / 100);
            }
        }
        if (UpdateETCO2)
        {
            if (DateTime.Now < ETCO2UpdateEndTime && patient.EtPeak != ETCO2Final)
            {
                if (DateTime.Now > NextETCO2Reading)
                {
                    patient.EtPeak = (ETCO2rate * (float)(DateTime.Now - ETCO2UpdateStartTime).TotalMilliseconds) + ETCO2Initial;
                    ETCO2Text.text = Mathf.RoundToInt(patient.EtPeak).ToString();
                    NextETCO2Reading = DateTime.Now.AddSeconds(ETCO2UpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * ETCO2UpdateResolution * ETCO2UpdateResolutionVariationPercentage / 100);

                }
            }
            else
            {
                PatientObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), currentETCO2Event.evtCat + " " + currentETCO2Event.evtType + " Ended", currentETCO2Event.factor.ToString());
                patient.EtPeak = ETCO2Final;
                ETCO2Text.text = Mathf.RoundToInt(ETCO2Final).ToString();
                UpdateETCO2 = false;
            }
        }
        else
        {
            if (DateTime.Now > NextETCO2Reading)
            {
                patient.EtPeak = ETCO2Final + (float)(randomNumberGenerator.NextDouble() - 0.5) * ETCO2Range;
                ETCO2Text.text = Mathf.RoundToInt(patient.EtPeak).ToString();
                NextETCO2Reading = DateTime.Now.AddSeconds(ETCO2UpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * ETCO2UpdateResolution * ETCO2UpdateResolutionVariationPercentage / 100);
            }
        }

        //Updating redundant values
        if (DateTime.Now > NextTSKINReading)
        {
            patient.Tskin = TSKINFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * TSKINRange;
            TSKINText.text = string.Format("{0:0.0}", patient.Tskin);
            NextTSKINReading = DateTime.Now.AddSeconds(TSKINUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * TSKINUpdateResolution * TSKINUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextpPeakReading)
        {
            patient.Ppeak = pPeakFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * pPeakRange;
            pPeakText.text = Mathf.RoundToInt(patient.Ppeak).ToString();
            NextpPeakReading = DateTime.Now.AddSeconds(pPeakUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * pPeakUpdateResolution * pPeakUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextpMeanReading)
        {
            patient.Pmean = pMeanFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * pMeanRange;
            pMeanText.text = Mathf.RoundToInt(patient.Pmean).ToString();
            NextpMeanReading = DateTime.Now.AddSeconds(pMeanUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * pMeanUpdateResolution * pMeanUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextpeepReading)
        {
            patient.Peep = peepFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * peepRange;
            peepText.text = Mathf.RoundToInt(patient.Peep).ToString();
            NextpeepReading = DateTime.Now.AddSeconds(peepUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * peepUpdateResolution * peepUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NexttvExpmLReading)
        {
            patient.TVexpml = tvExpmLFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * tvExpmLRange;
            tvExpmLText.text = Mathf.RoundToInt(patient.TVexpml).ToString();
            NexttvExpmLReading = DateTime.Now.AddSeconds(tvExpmLUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * tvExpmLUpdateResolution * tvExpmLUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextmVspontReading)
        {
            patient.MvSpont = mVspontFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * mVspontRange;
            mVspontText.text = string.Format("{0:0.0}",patient.MvSpont);
            NextmVspontReading = DateTime.Now.AddSeconds(mVspontUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * mVspontUpdateResolution * mVspontUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextmVReading)
        {
            patient.Mv = mVFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * mVRange;
            mVText.text = string.Format("{0:0.0}", patient.Mv);
            NextmVReading = DateTime.Now.AddSeconds(mVUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * mVUpdateResolution * mVUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextfiReading)
        {
            patient.FiVal = fiFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * fiRange;
            if (fiFinal > 0 && patient.FiVal < 0)
            { patient.FiVal = 0; }
            fiText.text = string.Format("{0:0.0}", patient.FiVal);
            NextfiReading = DateTime.Now.AddSeconds(fiUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * fiUpdateResolution * fiUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextisoetReading)
        {
            patient.IsoEt = isoetFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * isoetRange;
            if (isoetFinal > 0 && patient.IsoEt < 0)
            { patient.IsoEt = 0; }
            isoetText.text = string.Format("{0:0.0}", patient.IsoEt);
            NextisoetReading = DateTime.Now.AddSeconds(isoetUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * isoetUpdateResolution * isoetUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextisofiReading)
        {
            patient.IsoFi = isofiFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * isofiRange;
            if (isofiFinal > 0 && patient.IsoFi < 0)
            { patient.IsoFi = 0; }
            isofiText.text = string.Format("{0:0.0}", patient.IsoFi);
            NextisofiReading = DateTime.Now.AddSeconds(isofiUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * isofiUpdateResolution * isofiUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > Nextmac62yReading)
        {
            patient.Mac = mac62yFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * mac62yRange;
            if (mac62yFinal > 0 && patient.Mac < 0)
            { patient.Mac = 0; }
            mac62yText.text = string.Format("{0:0.0}", patient.Mac);
            Nextmac62yReading = DateTime.Now.AddSeconds(mac62yUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * mac62yUpdateResolution * mac62yUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > Nexto2etReading)
        {
            patient.O2Et = o2etFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * o2etRange;
            o2etText.text = Mathf.RoundToInt(patient.O2Et).ToString();
            Nexto2etReading = DateTime.Now.AddSeconds(o2etUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * o2etUpdateResolution * o2etUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > Nexto2fiReading)
        {
            patient.O2Fi = o2fiFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * o2fiRange;
            o2fiText.text = Mathf.RoundToInt(patient.O2Fi).ToString();
            Nexto2fiReading = DateTime.Now.AddSeconds(o2fiUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * o2fiUpdateResolution * o2fiUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextCO2RRminReading)
        {
            patient.EtRR = CO2RRminFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * CO2RRminRange;
            CO2RRminText.text = Mathf.RoundToInt(patient.EtRR).ToString();
            NextCO2RRminReading = DateTime.Now.AddSeconds(CO2RRminUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * CO2RRminUpdateResolution * CO2RRminUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextcomplcmH2OReading)
        {
            patient.ComplCmH2o = complcmH2OFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * complcmH2ORange;
            complcmH2OText.text = Mathf.RoundToInt(patient.ComplCmH2o).ToString();
            NextcomplcmH2OReading = DateTime.Now.AddSeconds(complcmH2OUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * complcmH2OUpdateResolution * complcmH2OUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > Nexto2percentReading)
        {
            patient.O2percent = o2percentFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * o2percentRange;
            o2percentText.text = Mathf.RoundToInt(patient.O2percent).ToString();
            Nexto2percentReading = DateTime.Now.AddSeconds(o2percentUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * o2percentUpdateResolution * o2percentUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NexttotalflowReading)
        {
            patient.TotalFlow = totalflowFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * totalflowRange;
            totalflowText.text = string.Format("{0:0.00}", patient.TotalFlow);
            NexttotalflowReading = DateTime.Now.AddSeconds(totalflowUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * totalflowUpdateResolution * totalflowUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextpinspReading)
        {
            patient.Pinsp = pinspFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * pinspRange;
            pinspText.text = Mathf.RoundToInt(patient.Pinsp).ToString();
            NextpinspReading = DateTime.Now.AddSeconds(pinspUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * pinspUpdateResolution * pinspUpdateResolutionVariationPercentage / 100);
        }
        if (DateTime.Now > NextRRReading)
        {
            patient.VentRR = RRFinal + (float)(randomNumberGenerator.NextDouble() - 0.5) * RRRange;
            RRText.text = Mathf.RoundToInt(patient.VentRR).ToString();
            NextRRReading = DateTime.Now.AddSeconds(RRUpdateResolution + (float)(randomNumberGenerator.NextDouble() - 0.5) * RRUpdateResolution * RRUpdateResolutionVariationPercentage / 100);
        }


    }

    internal void AddRange(string rangeLine)
    {
        // split the string into tokens
        string[] rangeSplit = rangeLine.Split(' ', '(', ';', ')');

        // save category and type
        string rangeCat = rangeSplit[0];
        string range = rangeSplit[3];
        string rangeResolution = rangeSplit[4];
        string rangeResolutionVariation = rangeSplit[5];

        switch (rangeCat)
        {
            case "HR":
                HRRange = float.Parse(range);
                HRUpdateResolution = float.Parse(rangeResolution);
                HRUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "Pulse":
                PulseRange = float.Parse(range);
                PulseUpdateResolution = float.Parse(rangeResolution);
                PulseUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "SPO2":
                SPO2Range = float.Parse(range);
                SPO2UpdateResolution = float.Parse(rangeResolution);
                SPO2UpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "BP":
                BPRange = float.Parse(range);
                BPUpdateResolution = float.Parse(rangeResolution);
                BPUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "ETCO2":
                ETCO2Range = float.Parse(range);
                ETCO2UpdateResolution = float.Parse(rangeResolution);
                ETCO2UpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "TSKIN":
                TSKINRange = float.Parse(range);
                TSKINUpdateResolution = float.Parse(rangeResolution);
                TSKINUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "pPeak":
                pPeakRange = float.Parse(range);
                pPeakUpdateResolution = float.Parse(rangeResolution);
                pPeakUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "pMean":
                pMeanRange = float.Parse(range);
                pMeanUpdateResolution = float.Parse(rangeResolution);
                pMeanUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "peep":
                peepRange = float.Parse(range);
                peepUpdateResolution = float.Parse(rangeResolution);
                peepUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "tvExpmL":
                tvExpmLRange = float.Parse(range);
                tvExpmLUpdateResolution = float.Parse(rangeResolution);
                tvExpmLUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "mVspont":
                mVspontRange = float.Parse(range);
                mVspontUpdateResolution = float.Parse(rangeResolution);
                mVspontUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "mV":
                mVRange = float.Parse(range);
                mVUpdateResolution = float.Parse(rangeResolution);
                mVUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "fi":
                fiRange = float.Parse(range);
                fiUpdateResolution = float.Parse(rangeResolution);
                fiUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "isoet":
                isoetRange = float.Parse(range);
                isoetUpdateResolution = float.Parse(rangeResolution);
                isoetUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "isofi":
                isofiRange = float.Parse(range);
                isofiUpdateResolution = float.Parse(rangeResolution);
                isofiUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "mac62y":
                mac62yRange = float.Parse(range);
                mac62yUpdateResolution = float.Parse(rangeResolution);
                mac62yUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "o2et":
                o2etRange = float.Parse(range);
                o2etUpdateResolution = float.Parse(rangeResolution);
                o2etUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "o2fi":
                o2fiRange = float.Parse(range);
                o2fiUpdateResolution = float.Parse(rangeResolution);
                o2fiUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "CO2RRmin":
                CO2RRminRange = float.Parse(range);
                CO2RRminUpdateResolution = float.Parse(rangeResolution);
                CO2RRminUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "complcmH2O":
                complcmH2ORange = float.Parse(range);
                complcmH2OUpdateResolution = float.Parse(rangeResolution);
                complcmH2OUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "o2percent":
                o2percentRange = float.Parse(range);
                o2percentUpdateResolution = float.Parse(rangeResolution);
                o2percentUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "totalflow":
                totalflowRange = float.Parse(range);
                totalflowUpdateResolution = float.Parse(rangeResolution);
                totalflowUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "pinsp":
                pinspRange = float.Parse(range);
                pinspUpdateResolution = float.Parse(rangeResolution);
                pinspUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
            case "RR":
                RRRange = float.Parse(range);
                RRUpdateResolution = float.Parse(rangeResolution);
                RRUpdateResolutionVariationPercentage = float.Parse(rangeResolutionVariation);
                break;
        }

    }
}


