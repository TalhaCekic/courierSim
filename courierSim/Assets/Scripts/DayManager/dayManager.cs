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
            
            DayOfCheck();
            HourPrint();
            DirectionLight();
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

    // zaman yazdırmak için
    private void HourPrint()
    {
        switch (minute)
        {
            case < 10:
                MinuteText.text = "0" + minute.ToString();
                break;
            case >= 10:
                MinuteText.text = minute.ToString();
                break;
        }

        switch (hour)
        {
            case < 10:
                HourText.text = "0" + hour.ToString();
                break;
            case >= 10:
                HourText.text = hour.ToString();
                break;
        }
    }

    private void DirectionLight()
    {
        switch (hour)
        {
            case <6:
                directionLight.transform.Rotate(Vector3.right, 0 * Time.deltaTime);
                directionLight.intensity = 0;
                break;
            case <12:
                directionLight.transform.Rotate(Vector3.right, 0.7f * Time.deltaTime);
                directionLight.intensity = 1.5f;
                break;
            case <18:
                directionLight.transform.Rotate(Vector3.right, 0 * Time.deltaTime);
                directionLight.intensity = 1;
                break;
            case <22:
                directionLight.transform.Rotate(Vector3.right, 0.7f * Time.deltaTime);
                directionLight.intensity = 1;
                break;   
        }
    }
}