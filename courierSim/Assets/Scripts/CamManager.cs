using Cinemachine;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    private CinemachineFreeLook cinema;

    void Start()
    {
        cinema = GetComponent<CinemachineFreeLook>();
        cinema.enabled = false;
    }

    void Update()
    {
        this.transform.position = Vector3.zero;
        if (interact.instance.isMotor)
        {
           // Camera.main.transform.SetParent(null);
            if (!interact.instance.isChangeCameraPov)
            {
                cinema.enabled = true;
            }
            else
            {
                cinema.enabled = false;
            }
        }
    }
}