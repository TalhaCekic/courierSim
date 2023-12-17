using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motoTransport : MonoBehaviour
{
    public GameObject boxCover;

    void Start()
    {
        boxCover = GameObject.Find("cover");
    }

    void Update()
    {
        float rotationSpeed = 10;
        if (interact.instance.isBoxOpen)
        {
            boxCover.transform.localRotation = Quaternion.Lerp(boxCover.transform.localRotation, Quaternion.Euler(80, 0, 0),
                Time.deltaTime * rotationSpeed);
        }
        else
        {
          boxCover.transform.localRotation = Quaternion.Lerp(boxCover.transform.localRotation, Quaternion.Euler(8, 0, 0),
              Time.deltaTime * rotationSpeed);
        }
    }
}