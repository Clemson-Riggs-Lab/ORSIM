using UnityEngine;
using System.Collections;
using System;

public class PopupWindow : MonoBehaviour
{
    // 200x300 px window will apear in the center of the screen.
    // private Rect windowRect = new Rect((Screen.width - 400) / 2, (Screen.height - 600) / 2, 400, 400);
    // Only show it if needed.
    //setting window on the buttom
    private Rect windowRect = new Rect((Screen.width - 400) / 2, /*(Screen.height) / 2*/ 700, 400, 350);
    private bool show = false;
    private string EventType;
    private bool FirstOptionShow;
    private GUIStyle currentStyle=null;

    void OnGUI()
    {


        //GUI.backgroundColor = Color.green;
        GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 25;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;

        if (show)
        windowRect = GUI.Window(0, windowRect, DialogWindow,"");
    }

    // This is the actual window.
    void DialogWindow(int windowID)
    {
        float y = 20;
        GUI.Label(new Rect(0, y, windowRect.width - 20, 80), "What type of change in " + EventType + " \n did you notice");
        if (FirstOptionShow)
        if (GUI.Button(new Rect(5, y+85, windowRect.width - 20, 40), "Increase"))
        {
                this.gameObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), "Selected "+EventType+" Changed", "Increase");
                show = false;
        }

        if (GUI.Button(new Rect(5, y+145, windowRect.width - 20, 40), "Decrease"))
        {
            this.gameObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), "Selected " + EventType + " Changed", "Decrease");
            show = false;
        }

        if (GUI.Button(new Rect(5, y + 205, windowRect.width - 20, 40), "Normalized"))
        {
            this.gameObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), "Selected " + EventType + " Changed", "Normalized");
            show = false;
        }
        if (GUI.Button(new Rect(5, y + 265, windowRect.width - 20, 40), "Cancel"))
        {
            this.gameObject.GetComponent<CsvReadWrite>().AddEvent(DateTime.Now.ToString("HH:mm:ss:ffff"), "Selected " + EventType + " Changed", "Cancel");
            show = false;
        }
    }

    // To open the dialogue from outside of the script.
    public void Open(string eventType )
    {
        EventType = eventType;
        //these were commented because dr riggs wanted to keep the increased option for the SPO2 as well.
        //if (eventType == "SPO2")
        //    FirstOptionShow = false;
        //else
            FirstOptionShow = true;
        show = true;
    }

}