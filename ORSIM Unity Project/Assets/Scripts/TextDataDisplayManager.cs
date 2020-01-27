using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * Text Data Display Manager
 * Authors: Julian Dixon
 * Last Update: July 21 2016
 * 
 * This class handles the displaying of all data in the anesthesia monitor
 * I found this to be a simpler way of handling this than having separate
 * classes for each graph like I did in the patient monitor
 */
public class TextDataDisplayManager : MonoBehaviour {

    // displayed in paw cmh2o panel
    public Text ppeakText;
    public Text pmeanText;
    public Text pawpeepText;

    // displayed in flow panel
    public Text tvExpMlText;
    public Text mVspontText;
    public Text mVText;

    // displayed in co2 mmhg panel
    public Text EtText;
    public Text FiText;

    // displayed in resp panel
    public Text co2rrText;
    public Text complmlcmh2oText;

    // displayed in gases panel
    public Text o2etText;
    public Text o2fiText;

    // displayed in agent panel
    public Text isoetText;
    public Text isofiText;
    public Text macText;

    // displayed in Fresh Gas panel
    public Text o2percentText;
    public Text totalFlowText;

    // displayed in ventilator panel
    public Text modeText;
    public Text pinspText;
    public Text ventRRText;
    public Text flowcontrolText;
    public Text prsupportText;
    public Text ventpeepText;

    public Text timeText;

    private Patient patient;

	// Use this for initialization
	void Start ()
    {
        patient = Patient.Instance;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    // displayed in paw cmh2o panel
        //ppeakText.text = patient.Ppeak.ToString();
        //pmeanText.text = patient.Pmean.ToString();
        //pawpeepText.text = patient.Peep.ToString();

        //// displayed in flow panel
        //tvExpMlText.text = patient.TVexpml.ToString();
        //mVspontText.text = patient.MvSpont.ToString();
        //mVText.text = patient.Mv.ToString();

        //// displayed in co2 mmhg panel
        //EtText.text = patient.EtPeak.ToString();
        //FiText.text = patient.FiVal.ToString();

        //// displayed in resp panel
        //co2rrText.text = patient.EtRR.ToString();
        //complmlcmh2oText.text = patient.ComplCmH2o.ToString();

        //// displayed in gases panel
        //o2etText.text = patient.O2Et.ToString();
        //o2fiText.text = patient.O2Fi.ToString();

        //// displayed in agent panel
        //isoetText.text = patient.IsoEt.ToString();
        //isofiText.text = patient.IsoFi.ToString();
        //macText.text = patient.Mac.ToString();

        //// displayed in Fresh Gas panel
        //o2percentText.text = patient.O2percent.ToString();
        //totalFlowText.text = patient.TotalFlow.ToString();

        //// displayed in ventilator panel
        //modeText.text = patient.VentMode;
        //pinspText.text = patient.Pinsp.ToString();
        //ventRRText.text = patient.VentRR.ToString();
        //flowcontrolText.text = patient.FlowControl.ToString();
        //prsupportText.text = patient.PRsupport.ToString();
        //ventpeepText.text = patient.Peep.ToString();

        timeText.text = System.DateTime.Now.ToShortTimeString();
    }
}
