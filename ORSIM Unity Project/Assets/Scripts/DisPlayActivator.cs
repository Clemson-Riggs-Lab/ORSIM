using UnityEngine;
using System.Collections;

/* DisplayActivator
 * Authors: Julian Dixon
 * Last Update: July 12 2016
 * 
 * This class activates the second display (the display where the anesthesia monitor is
 * shown) there is also functionality for the exit button here since theres not a better
 * place for it
 */

/*
* this code was commented by jawad alami, because it was forcing the simulation to run on
* both screens with and empty second monitor after we have merged the two scenes( patient and anesthesia) into a single monitor.
* you can get the previous behavior through just uncommenting the code below.
*/

public class DisPlayActivator : MonoBehaviour {

   //// Use this for initialization
   //void Start ()
//   {
//       if (Display.displays.Length > 1)
//       {
//           Display.displays[1].Activate();          
//       }
//   }

   //// Update is called once per frame
   //void Update () {

   //}

//   // exit the application.
//   // TODO Fix this, it doesn't work...
//   public void SetupQuitSelected()
//   {
//       Application.Quit();
//   }
}
