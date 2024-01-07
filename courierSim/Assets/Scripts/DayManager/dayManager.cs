using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

public class dayManager : MonoBehaviour
{
    public static dayManager instance;
    public Light directionLight;

    //public float Time;
    public bool isDayOn;
    public bool isHourOn;
    public bool isdayFinished;
    public int day;
    public int hour;
    public int minute;
    public float minuteF;

    public TMP_Text HourText;
    public TMP_Text MinuteText;

    void Start()
    {
        instance = this;
        hour = 08;
        minute = 00;
    }

    void Update()
    {
        // TR
        if (isDayOn)
        {
            minuteF += Time.deltaTime * 2;
            minute = Mathf.RoundToInt(minuteF);
            
                directionLight.transform.Rotate(Vector3.right, 0.05f * Time.deltaTime);
                
            if (minuteF > 60)
            {
                isHourOn = true;
            }
            if (isHourOn)
            {
                minuteF = 0;
                hour++;
                isHourOn = false;
            }
            HourText.text = hour.ToString();
            MinuteText.text = minute.ToString();
            DayOfCheck();
        }
        else
        {
            
        }
    }

    private void DayOfCheck()
    {
        if (hour == 24)
        {
            isDayOn = false;
            isdayFinished = true;
        }
    }
}