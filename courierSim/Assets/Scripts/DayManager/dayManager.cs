using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class dayManager : MonoBehaviour
{
    public static dayManager instance;
    [SerializeField] private Light sun;

    //[SerializeField, Range(0, 24)] private float timeOfDay;

    [SerializeField] private float sunRotationSpeed;

    [Header("LightingPreset")] [SerializeField]
    private Gradient skyColor;

    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;

    private float newRotation;
    private float currentRotation;
    private Quaternion startRotation;
    private Quaternion endRotation;
    private bool hasRotated;
    private float rotationTime;
    private bool changeTime;

    //public float Time;
    public bool isDayOn;
    public bool isNightDay;
    public bool isHourOn;
    public bool isdayFinished;
    public int day;
    [SerializeField, Range(0, 24)] public int hour;
    [SerializeField, Range(0, 24)] public float timeOfDay;
    public int minute;
    public float minuteF;

    public TMP_Text HourText;
    public TMP_Text MinuteText;

    //sleep
    public Image SleepingBg;
    private Color NewColor;
    public float sleepingUıSpeed;
    public float sleepUI ;
    private float sleepDelay;
    public bool isSleeping;
    public float delay;

    public GameObject endDayCanvas;

    void Start()
    {
        instance = this;
        isNightDay = false;
        hour = 07;
        timeOfDay = 7;
        minute = 00;
        sunRotationSpeed = 1;
        SleepingBg.gameObject.SetActive(false);
        endDayCanvas.SetActive(false);
    }

    private void UpdateSunRotation()
    {
        float sunRotation = Mathf.Lerp(-90, 270, timeOfDay / 24);
        sun.transform.rotation = Quaternion.Euler(sunRotation, sun.transform.rotation.y, sun.transform.rotation.z);
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
        HourPrint();
        DayOfCheck();
        UpdateSunRotation();
        UpdateLighting();
        SleepingUI();
        EndTheDay();
        // TR
        if (isDayOn)
        {
            timeOfDay += Time.deltaTime / 30 * sunRotationSpeed;

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
        }
        else
        {
        }
    }

    private void DayOfCheck()
    {
        if (hour >= 24)
        {
            hour = 0;
            timeOfDay = 0;
            minuteF = 0;
            minute = 0;
            sunRotationSpeed = 0;
        }

        if (hour >= 17 || hour < 7)
        {
            isNightDay = true;
        }
        else
        {
            isNightDay = false;
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

    // sleeping
    public void SleepSystem()
    {
        if (hour + 8 < 24)
        {
            if (hour < hour + 8)
            {
                changeTime = true;
                // hour += 8;
                // timeOfDay += 8;
                 isSleeping = true;
                // hour = Mathf.RoundToInt(timeOfDay);
            }
        }
        else
        {
            changeTime = true;
            // hour = hour + 8 - 24;
            // timeOfDay = timeOfDay + 8 - 24;
            // hour = Mathf.RoundToInt(timeOfDay);
             isSleeping = true;
            // isdayFinished = true;
            // isDayOn = false;
        }
    }

    private void SleepingUI()
    {
        if (isSleeping && !isdayFinished && changeTime)
        {
            SleepingBg.gameObject.SetActive(true);
            sleepUI += sleepingUıSpeed * Time.deltaTime;
            NewColor = SleepingBg.color;
            NewColor.a = sleepUI;
            SleepingBg.color = NewColor;
            sunRotationSpeed = 0;
            if (sleepUI >= 1)
            {
                sleepUI = 1;
                delay += Time.deltaTime;
                if (delay >= 2)
                {
                    isSleeping = false;
                    delay = 0;
                    if (hour + 8 < 24)
                    {
                        if (hour < hour + 8)
                        {
                            print("a");
                            hour += 8;
                            timeOfDay += 8;
                            hour = Mathf.RoundToInt(timeOfDay);
                        }
                    }
                    else
                    {
                        changeTime = true;
                        hour = hour + 8 - 24;
                        timeOfDay = timeOfDay + 8 - 24;
                        hour = Mathf.RoundToInt(timeOfDay);
                 
                    }
                    changeTime = false;
                }
            }
        }
        else if(!isSleeping && !changeTime)
        {
            if (sleepUI > 0.1f)
            {
                sleepUI -= sleepingUıSpeed * Time.deltaTime;
                NewColor = SleepingBg.color;
                NewColor.a = sleepUI;
                SleepingBg.color = NewColor;
                sunRotationSpeed = 1;
                isSleeping = false;
            }
            else if(sleepUI<0.1f)
            {
                SleepingBg.gameObject.SetActive(false);
            }
        }
    }

    // gün sonu
    private void EndTheDay()
    {
        if (isdayFinished && !isDayOn)
        {
            endDayCanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    //butona verlen devam et etkisi
    public void notEndTheDay()
    {
        isdayFinished = false;
        isDayOn = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}