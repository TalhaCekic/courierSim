using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class dayManager : MonoBehaviour
{
    public static dayManager instance;
    public Light directionLight;
    private float newRotation;
    private float currentRotation;
    private Quaternion startRotation;
    private Quaternion endRotation;
    private bool hasRotated;
    private float rotationTime;

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

    public float timeSpeed;

    void Start()
    {
        instance = this;
        hour = 07;
        minute = 00;
        timeSpeed = 1;
    }

    void Update()
    {
        TimeSettings();
        // TR
        if (isDayOn)
        {
            minuteF += Time.deltaTime * 2 * timeSpeed;
            minute = Mathf.RoundToInt(minuteF);

            if (minuteF > 60)
            {
                isHourOn = true;
            }

            if (isHourOn)
            {
                minuteF = 0;
                hour++;
                hasRotated = false;
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
            hour = 0;
            minuteF = 0;
            minute = 0;
            timeSpeed = 0;
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
            case 7:
                ChangeRotate();
                break;
            case 8:
                ChangeRotate();
                break;
            case 9:
                ChangeRotate();
                break;
            case 10:
                ChangeRotate();
                break;
            case 11:
                ChangeRotate();
                break;
            case 12:
                ChangeRotate();
                break;
            case 13:
                ChangeRotate();
                break;
            case 14:
                ChangeRotate();
                break;
            case 15:
                ChangeRotate();
                break;
            case 16:
                ChangeRotate();
                break;
            case 17:
                ChangeRotate();
                break;
            case 18:
                ChangeRotate();
                break;
            case 19:
                ChangeRotate();
                break;
            case 20:
                ChangeRotate();
                break;
            case 21:
                ChangeRotate();
                break;
            case 22:
                ChangeRotate();
                break;
            case 23:
                ChangeRotate();
                break;
            case 00:
                ChangeRotate();
                break;
        }
    }

    private void TimeSettings()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            timeSpeed = 0;
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            timeSpeed = 1;
            isdayFinished = false;
            isDayOn = true;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            timeSpeed = 2;
            isdayFinished = false;
            isDayOn = true;
        }
        
        if (Input.GetKey(KeyCode.Alpha3))
        {
            timeSpeed = 3;
            isdayFinished = false;
            isDayOn = true;
        } 
        if (Input.GetKey(KeyCode.Alpha4))
        {
            timeSpeed = 6;
            isdayFinished = false;
            isDayOn = true;
        }
    }

    private void ChangeRotate()
    {
        // currentRotation = directionLight.transform.rotation.eulerAngles.x;
        //
        // if (!hasRotated)
        // {
        //     newRotation = currentRotation + 18f;
        //     hasRotated = true;
        //     print("current" + currentRotation);
        //     print("newRotatin " + newRotation);
        // }
        //
        // startRotation = Quaternion.Euler(currentRotation, 0f, 0f);
        // endRotation = Quaternion.Euler(newRotation, 0f, 0f);
        //
        // directionLight.transform.rotation = Quaternion.Lerp(startRotation ,endRotation,Time.deltaTime/7 *timeSpeed);
        
        
        //
        
        currentRotation = directionLight.transform.localRotation.eulerAngles.x;
        if (!hasRotated)
        {
            newRotation = currentRotation + 18f;
            hasRotated = true;
            print("current: " + currentRotation);
            print("newRotation: " + newRotation);
            rotationTime = 0f;  
        }
        
        startRotation = Quaternion.Euler(currentRotation, 0f, 0f);
        endRotation = Quaternion.Euler(newRotation, 0f, 0f);
        
        rotationTime += Time.deltaTime / 7 * timeSpeed;  
        directionLight.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, rotationTime);
    }
}