using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    private CinemachineFreeLook cinema;

    void Start()
    {
        cinema = GetComponent<CinemachineFreeLook>();
        //cinema.enabled = false;
    }

    void Update()
    {
        // if (interact.instance.isMotor)
        // {
        //    // Camera.main.transform.SetParent(null);
        //     if (!interact.instance.isChangeCameraPov)
        //     {
        //         cinema.enabled = true;
        //     }
        //     else
        //     {
        //         cinema.enabled = false;
        //     }
        // }
    }
}