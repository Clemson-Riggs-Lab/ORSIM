using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine.UI;

public class CsvReadWrite : MonoBehaviour
{

    private List<string[]> rowData = new List<string[]>();
    private string filepath;

    // Use this for initialization
    void Start()
    {
        SetUp();
        filepath =Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TactileOutput" + DateTime.Now.ToString("MM-dd-yy-H-mm") + ".csv");
    }

    void SetUp()
    {
        // Creating First row of titles manually..
        string[] rowDataTemp = new string[3];
        rowDataTemp[0] = "Time";
        rowDataTemp[1] = "EventType";
        rowDataTemp[2] = "EventDecision";
        rowData.Add(rowDataTemp);

    }
    public void AddEvent(string time, string actionType, string actionDecision)
    {
        string[] rowDataTemp = new string[3];

        rowDataTemp = new string[3];
        rowDataTemp[0] = time;
        rowDataTemp[1] = actionType;
        rowDataTemp[2] = actionDecision;
        rowData.Add(rowDataTemp);
        Save();
    }

   public void Save()
    {
 
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        StreamWriter outStream = System.IO.File.CreateText(filepath);
        outStream.WriteLine(sb);
        outStream.Close();
        //System.IO.File.WriteAllText(
    }

  
}