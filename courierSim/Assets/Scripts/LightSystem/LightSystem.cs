using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSystem : MonoBehaviour
{
    public GameObject LampGlass;
    public GameObject light;
    void Start()
    {
        LampGlass.SetActive(false);
        light.SetActive(false);
    }

    void Update()
    {
        if (dayManager.instance.isNightDay)
        {
            LampGlass.SetActive(true);
            light.SetActive(true);
        }
        else
        {
            LampGlass.SetActive(false);
            light.SetActive(false);
        }
    }
}
