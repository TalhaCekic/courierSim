using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class dayManager : MonoBehaviour
{
    public static dayManager instance;
    [SerializeField] private Light sun;

    //[SerializeField, Range(0, 24)] private float timeOfDay;

    [SerializeField] private float sunRotationSpeed;

    [Header("LightingPreset")]
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;
    
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
    [SerializeField,Range(0,24)] public float hour;
    [SerializeField,Range(0,24)] public float timeOfDay;
    public int minute;
    public float minuteF;

    public TMP_Text HourText;
    public TMP_Text MinuteText;
    

    void Start()
    {
        instance = this;
        hour = 07;
        timeOfDay = 7;
        minute = 00;
        sunRotationSpeed = 1;
    }

    private void UpdateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-90, 270, timeOfDay / 24);
        sun.transform.rotation = Quaternion.Euler(sunRotation,sun.transform.rotation.y,sun.transform.rotation.z);
    }
    private void OnValidate()
    {
        UpdateSunRotation();
        UpdateLighting();
    }

    private void UpdateLighting()
    {
        float timeFraction = timeOfDay / 24;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFraction);
        RenderSettings.ambientSkyColor = skyColor.Evaluate(timeFraction);
        sun.color = sunColor.Evaluate(timeFraction);
    }
    void Update()
    {
        TimeSettings();
        // TR
        if (isDayOn)
        {
            //güneş için float değişkeni hesaplat
            timeOfDay +=Time.deltaTime /30 *sunRotationSpeed;
            UpdateSunRotation();
            UpdateLighting();
            
            minuteF += Time.deltaTime * 2 * sunRotationSpeed;
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
            timeOfDay =0;
            minuteF = 0;
            minute = 0;
            sunRotationSpeed = 0;
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
        // switch (hour)
        // {
        //     case 7:
        //         ChangeRotate();
        //         break;
        //     case 8:
        //         ChangeRotate();
        //         break;
        //     case 9:
        //         ChangeRotate();
        //         break;
        //     case 10:
        //         ChangeRotate();
        //         break;
        //     case 11:
        //         ChangeRotate();
        //         break;
        //     case 12:
        //         ChangeRotate2();
        //         break;
        //     case 13:
        //         ChangeRotate2();
        //         break;
        //     case 14:
        //         ChangeRotate2();
        //         break;
        //     case 15:
        //         ChangeRotate2();
        //         break;
        //     case 16:
        //         ChangeRotate2();
        //         break;
        //     case 17:
        //         ChangeRotate2();
        //         break;
        //     case 18:
        //         directionLight2.intensity = 0;
        //         //ChangeRotate2();
        //         break;
        //     // case 19:
        //     //     ChangeRotate2();
        //     //     break;
        //     // case 20:
        //     //     ChangeRotate2();
        //     //     break;
        //     // case 21:
        //     //     ChangeRotate2();
        //     //     break;
        //     // case 22:
        //     //     ChangeRotate2();
        //     //     break;
        //     // case 23:
        //     //     ChangeRotate2();
        //     //     break;
        //     // case 00:
        //     //     ChangeRotate2();
        //     //     break;
        // }
    }

    private void TimeSettings()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            sunRotationSpeed = 0;
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            sunRotationSpeed = 1;
            isdayFinished = false;
            isDayOn = true;
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            sunRotationSpeed = 2;
            isdayFinished = false;
            isDayOn = true;
        }
        
        if (Input.GetKey(KeyCode.Alpha3))
        {
            sunRotationSpeed = 3;
            isdayFinished = false;
            isDayOn = true;
        } 
        if (Input.GetKey(KeyCode.Alpha4))
        {
            sunRotationSpeed = 6;
            isdayFinished = false;
            isDayOn = true;
        }
    }

    
}