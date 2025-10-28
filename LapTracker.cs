using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LapTracker : MonoBehaviour
{
    private float Timer;
    private float LastTime;
    private float BestTime = 0;

    private int currentminuteTimer;
    private float currentsecondTimer;

    private int lastminuteTimer = 0;
    private float lastsecondTimer = 0;

    private int bestminuteTimer = 0;
    private float bestsecondTimer = 0;

    private bool startTimer = false;

    [SerializeField] private bool Checkpoint1 = false;
    [SerializeField] private bool Checkpoint2 = false;
    [SerializeField] private bool Checkpoint3 = false;
    [SerializeField] private bool Checkpoint4 = false;
    [SerializeField] private bool Checkpoint5 = false;
    [SerializeField] private bool Checkpoint6 = false;
    [SerializeField] private bool Checkpoint7 = false;
    [SerializeField] private bool Checkpoint8 = false;
    [SerializeField] private bool Checkpoint9 = false;
    [SerializeField] private bool Checkpoint10 = false;
    [SerializeField] private bool Checkpoint11 = false;
    [SerializeField] private bool Checkpoint12 = false;
    [SerializeField] private bool Checkpoint13 = false;
    [SerializeField] private bool Checkpoint14 = false;
    [SerializeField] private bool Checkpoint15 = false;
    [SerializeField] private bool Checkpoint16 = false;
    [SerializeField] private bool Checkpoint17 = false;
    [SerializeField] private bool Checkpoint18 = false;
    [SerializeField] private bool Checkpoint19 = false;
    [SerializeField] private bool Checkpoint20 = false;
    [SerializeField] private bool Checkpoint21 = false;
    [SerializeField] private bool Checkpoint22 = false;
    [SerializeField] private bool Checkpoint23 = false;
    [SerializeField] private bool Checkpoint24 = false;
    [SerializeField] private bool Checkpoint25 = false;
    [SerializeField] private bool Checkpoint26 = false;
    [SerializeField] private bool Checkpoint27 = false;
    [SerializeField] private bool Checkpoint28 = false;
    [SerializeField] private bool Checkpoint29 = false;

    [SerializeField] private Text currentTime;
    [SerializeField] private Text lastlapTime;
    [SerializeField] private Text bestlapTime;

    [SerializeField] private bool showTimers=true;
    [SerializeField] private GameObject Laptimes;

    void Update()
    {
        //Lap Tracker
        if (startTimer == true) //Current Lap
        {
            Timer = Timer + Time.deltaTime;
            //Debug.Log(Timer);

            currentminuteTimer = Mathf.FloorToInt(Timer/60);
            currentsecondTimer = Timer%60;

            currentTime.text = "Current Lap: " + currentminuteTimer.ToString() + ":" + currentsecondTimer.ToString("F3");
        }

        if (currentsecondTimer<10)
        {
            currentTime.text = "Current Lap: " + currentminuteTimer.ToString() + ":" + "0" + currentsecondTimer.ToString("F3");
        }

        if (Input.GetKeyDown(KeyCode.Tab)){
            if (showTimers){
                showTimers=false;
                Laptimes.SetActive(false);
            }
            else
            {
                showTimers=true;
                Laptimes.SetActive(true);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "30" && Checkpoint1==true && Checkpoint2==true && Checkpoint3==true &&
        Checkpoint4==true && Checkpoint5==true && Checkpoint6==true &&
        Checkpoint7==true && Checkpoint8==true && Checkpoint9==true &&
        Checkpoint10==true && Checkpoint11==true && Checkpoint12==true &&
        Checkpoint13==true && Checkpoint14==true && Checkpoint15==true &&
        Checkpoint16==true && Checkpoint17==true && Checkpoint18==true &&
        Checkpoint19==true && Checkpoint20==true && Checkpoint21==true &&
        Checkpoint22==true && Checkpoint23==true && Checkpoint24==true &&
        Checkpoint25==true && Checkpoint26==true && Checkpoint27==true &&
        Checkpoint28==true && Checkpoint29==true)
        {
            startTimer = false;
            LastTime = Timer;

            lastminuteTimer = currentminuteTimer;
            lastsecondTimer = currentsecondTimer;

            lastlapTime.text = "Last Lap: " + lastminuteTimer.ToString() + ":" + lastsecondTimer.ToString("F3");

            if (lastsecondTimer<10)
            {
                lastlapTime.text = "Last Lap: " + lastminuteTimer.ToString() + ":" + "0" + lastsecondTimer.ToString("F3");
            }
            PersonalBest();
            Debug.Log("Finish");
        }

        if(other.gameObject.name == "0") {
            Timer = 0;
            startTimer = true;
            Debug.Log("Start");

            Checkpoint1 = false;
            Checkpoint2 = false;
            Checkpoint3 = false;
            Checkpoint4 = false;
            Checkpoint5 = false;
            Checkpoint6 = false;
            Checkpoint7 = false;
            Checkpoint8 = false;
            Checkpoint9 = false;
            Checkpoint10 = false;
            Checkpoint11 = false;
            Checkpoint12 = false;
            Checkpoint13 = false;
            Checkpoint14 = false;
            Checkpoint15 = false;
            Checkpoint16 = false;
            Checkpoint17 = false;
            Checkpoint18 = false;
            Checkpoint19 = false;
            Checkpoint20 = false;
            Checkpoint21 = false;
            Checkpoint22 = false;
            Checkpoint23 = false;
            Checkpoint24 = false;
            Checkpoint25 = false;
            Checkpoint26 = false;
            Checkpoint27 = false;
            Checkpoint28 = false;
            Checkpoint29 = false;
        }
        
        switch (Convert.ToInt16(other.gameObject.name)) {
            case 1: { Checkpoint1 = true; } break;
            case 2: {  Checkpoint2 = true; } break;
            case 3: {  Checkpoint3 = true; } break;
            case 4: {  Checkpoint4 = true; } break;
            case 5: {  Checkpoint5 = true; } break;
            case 6: {  Checkpoint6 = true; } break;
            case 7: {  Checkpoint7 = true; } break;
            case 8: {  Checkpoint8 = true; } break;
            case 9: {  Checkpoint9 = true; } break;
            case 10: {  Checkpoint10 = true; } break;
            case 11: {  Checkpoint11 = true; } break;
            case 12: {  Checkpoint12 = true; } break;
            case 13: {  Checkpoint13 = true; } break;
            case 14: {  Checkpoint14 = true; } break;
            case 15: {  Checkpoint15 = true; } break;
            case 16: {  Checkpoint16 = true; } break;
            case 17: {  Checkpoint17 = true; } break;
            case 18: {  Checkpoint18 = true; } break;
            case 19: {  Checkpoint19 = true; } break;
            case 20: {  Checkpoint20 = true; } break;
            case 21: {  Checkpoint21 = true; } break;
            case 22: {  Checkpoint22 = true; } break;
            case 23: {  Checkpoint23 = true; } break;
            case 24: {  Checkpoint24 = true; } break;
            case 25: {  Checkpoint25 = true; } break;
            case 26: {  Checkpoint26 = true; } break;
            case 27: {  Checkpoint27 = true; } break;
            case 28: {  Checkpoint28 = true; } break;
            case 29: {  Checkpoint29 = true; } break;
        }
    }

    public void PersonalBest()
    {
        if (BestTime==0)
        {
            BestTime = LastTime;
        }

        if (LastTime<BestTime)
        {
            BestTime = LastTime;
        }
        
        bestminuteTimer = Mathf.FloorToInt(BestTime/60);
        bestsecondTimer = BestTime%60;
        
        bestlapTime.text = "Best Lap: " + bestminuteTimer.ToString() + ":" + bestsecondTimer.ToString("F3");

        if (bestsecondTimer<10)
        {
            bestlapTime.text = "Last Lap: " + bestminuteTimer.ToString() + ":" + "0" + bestsecondTimer.ToString("F3");
        }
    }
}
