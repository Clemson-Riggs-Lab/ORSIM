using UnityEngine;
using System.Collections;
using System;

public class SPO2ChangedButtonClicked : MonoBehaviour
{
    public void PopUp()
    {
        this.gameObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), "Selected SPO2 Changed", "-");
        this.gameObject.GetComponent<PopupWindow>().Open("SPO2");
    }
}