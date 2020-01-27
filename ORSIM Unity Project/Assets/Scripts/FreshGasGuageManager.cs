using UnityEngine;
using System.Collections;
/*
 * Fresh Gas Guage Manager
 * Author: Julian Dixon
 * Last Update: July 21 2016
 * 
 * This class handles the vertical guage on the left side of the anesthesia monitor
 */

public class FreshGasGuageManager : GraphManager {

    private float air;
    private float oxygen;
	// Use this for initialization
	void Start () {
        air = 1.0f;
        oxygen = 1.0f;
        previousSum = oxygen;
        currentSum = air;
	}
	
	// Update is called once per frame
    // air just sits at 1.0
	void FixedUpdate () {
        currentSum = air;
	}
}
