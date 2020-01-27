using UnityEngine;
using System.Collections;
using System;

public class BPChangedButtonClicked : MonoBehaviour {
    public void PopUp()
    {
        this.gameObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), "Selected MAP Changed", "-");
        this.gameObject.GetComponent<PopupWindow>().Open("MAP");
    }
}