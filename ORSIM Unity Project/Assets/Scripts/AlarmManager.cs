using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/**
 * Original Author: Julian Dixon
 * Last Edited: July 12 2016
 * 
 * this script should be attached to an empty game object with an audiosource component
 * it will handle any events that should demand the attention of the user
 * 
 */
public class AlarmManager : MonoBehaviour
{
    // audio source and various clips
    // set in editor
    public AudioSource efxSource;
    public AudioClip leadsOffClip;
    public AudioClip asystoleClip;
    public AudioClip hralarmClip;
    public AudioClip bpalarmClip;
    public AudioClip spo2alarmClip;
    public AudioClip apneaClip;

    // notification bar stuff
    private Image notificationBackground;
    private Text notificationText;
    // start out text and background as invisible
    Color backgroundColor = Color.clear;
    Color textColor = Color.clear;

    // information about current alarm
    public bool playing;

    // used to flash the alarm text
    private bool textVisible;
    
    private int severity;

    public bool disabled;

    // hold our continuous monitor so we can pasue it when an alarm is playing
    private ContinousMonitor continuosMonitor;

    void Awake()
    {
        // get our variables and set some of the boolean properties
        continuosMonitor = GameObject.FindGameObjectWithTag("ContMonitor").GetComponent<ContinousMonitor>();
        notificationBackground = GameObject.FindGameObjectWithTag("AlarmNotify").GetComponent<Image>();
        notificationText = GameObject.FindGameObjectWithTag("AlarmNotify").GetComponentInChildren<Text>();
        DontDestroyOnLoad(gameObject);
        efxSource = GetComponent<AudioSource>();

        playing = false;
        disabled = false;
        notificationBackground.color = Color.clear;
        notificationText.text = "";
    }

    // regular update function to handle the visual notification
    void Update()
    {
        if (severity == 3)
        {
            backgroundColor = Color.red;
            textColor = Color.white;
        }
        else if (severity == 2)
        {
            backgroundColor = Color.yellow;
            textColor = Color.red;
        }
    }

    // fixed update for handling the playing of alarm sounds
    void FixedUpdate()
    {
        if (disabled)
        {
            
        }
        else
        {
            if (playing)
            {
                StartCoroutine(FlashText());
            }
            else
            {
               // leave backround and text as is
                notificationBackground.color = Color.clear;
                notificationText.text = "";
                continuosMonitor.Unmute();
            }
        }
    }

    // invoked by other objects that need to raise an alarm
    public void StartAlarm(int type)
    {
        playing = true;
        // mute the continous monitor
        //continuosMonitor.Mute();

        // determine which clip to play
         if (type == (int)EventTypes.SPO2LOW)
        {
            efxSource.clip = spo2alarmClip;
            //notificationText.text = "**SPO2LOW";
            //severity = 2;
        }
        /// the alarms below were commented by jawad on the request of kylie
        /// They were commented because simulating the operating room, alarms are only present for SPO2 decrease and HR change.
        /// HR alarms were also commented because kylie said that we are not presenting HR change cues, and HR changes with SPO2 in 
        /// our scenarios, so they would trigger together, and thus there isnt a need for the HR alarm

        //else if (type == (int)EventTypes.HRHIGH)
        //{
        //    efxSource1.clip = hralarmClip;
        //    notificationText.text = "**HR HIGH";
        //    severity = 2;
            
        //}
        //else if (type == (int)EventTypes.HRLOW)
        //{
        //    efxSource1.clip = hralarmClip;
        //    notificationText.text = "**HR LOW";
        //    severity = 2;
        //}
        //else if (type == (int)EventTypes.XBRADY)
        //{

        //    notificationText.text = "***EXTREME BRADY";
        //    severity = 3;
        //}
        //else if (type == (int)EventTypes.XTACHY)
        //{
        //    notificationText.text = "***EXTREME TACHY";
        //    severity = 3;
        //}
        //else if (type == (int)EventTypes.ASYSTOLE)
        //{
        //    efxSource1.clip = asystoleClip;
        //    notificationText.text = "***ASYSTOLE";
        //    severity = 3;
        //}
        //else if (type == (int)EventTypes.ARHYTHMIA)
        //{
        //    notificationText.text = "***ARHYTHMIA";
        //    severity = 3;
        //}
        
        //else if (type == (int)EventTypes.BPHIGH)
        //{
        //    efxSource1.clip = bpalarmClip;
        //    notificationText.text = "**BP HIGH";
        //    severity = 2;
        //}
        //else if (type == (int)EventTypes.BPLOW)
        //{
        //    efxSource1.clip = bpalarmClip;
        //    notificationText.text = "**BP LOW";
        //    severity = 2;
        //}
         

        // start the sound
        efxSource.Play();
    }

    // stop playing the alarm
    public void StopAlarm()
    {
        playing = false;
        notificationBackground.color = Color.clear;
        notificationText.text = "";
        efxSource.Stop();
    }

    public void VolumeDown(float f)
    {
        efxSource.mute = false;
        efxSource.volume -= f;
    }

    public void VolumeUp(float f)
    {
        efxSource.mute = false;
        efxSource.volume += f;
    }

    public void Silence()
    {
        efxSource.mute = true;
    }

    // disable any alarms from being raised
    public void AlarmSwitch()
    {
        if (!disabled)
        {
            efxSource.mute = true;
            notificationBackground.color = Color.white;
            notificationText.color = Color.red;
            notificationText.text = "Alarms Off";
            disabled = true;
        }
        else
        {
            efxSource.mute = false;
            notificationBackground.color = Color.clear;
            notificationText.color = Color.clear;
            disabled = false;
        }
    }



    // thread to flash the text to give a more visual queue of the alarm
    public IEnumerator FlashText()
    {
        notificationBackground.color = backgroundColor;
        // wait for 0.5 seconds before flashing, otherwise it looks really ugly
        //if (textVisible)
        //{
        //    yield return new WaitForSeconds(0.5f);
        //    notificationText.color = Color.clear;
        //    textVisible = false;
            
        //}
        //else
        //{
            yield return new WaitForSeconds(0.5f);
            notificationText.color = textColor;
            textVisible = true;
            
        //}
    }
}
