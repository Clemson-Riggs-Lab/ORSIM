using UnityEngine;
using System.Collections;
using System;

public class ETCO2ChangedButtonClicked : MonoBehaviour {

    public void PopUp()
    {
        this.gameObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), "Selected ETCO2 Changed", "-");

        this.gameObject.GetComponent<PopupWindow>().Open("ETCO2");
    }
}
