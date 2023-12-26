using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class NPCcarMovement : MonoBehaviour
{
    public Transform Path;
    public float maxSteerAngle = 50;
    private float newSteer;
    public WheelCollider wheelFl;
    public WheelCollider wheelFr;
    public GameObject wheelFlObj;
    public GameObject wheelFrObj;

    public List<Transform> nodes;
    public int currentNode = 0;

    private void Start()
    {
        Transform[] pathTransform = Path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();
        for (int i = 0; i < pathTransform.Length; i++)
        {
            if (pathTransform[i] != Path.transform)
            {
                nodes.Add(pathTransform[i]);
            }
        }
    }

    private void FixedUpdate()
    {
        ApplySteer();

        CheckWayPointDistance();
        Drive();
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFl.steerAngle = newSteer;
        // wheelFlObj.transform.rotation = wheelFl.transform.rotation;
        // wheelFlObj.transform.position = wheelFl.transform.position;
        // wheelFrObj.transform.rotation = wheelFr.transform.rotation;
        // wheelFrObj.transform.position = wheelFr.transform.position;
        wheelFr.steerAngle = newSteer;
    }

    private void Drive()
    {
        wheelFl.motorTorque = 100;
        wheelFr.motorTorque = 100;
    }

    private void CheckWayPointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 0.5f)
        {
            currentNode++;
            if (currentNode >= nodes.Count)
            {
                currentNode = 0;
            }
        }
    }
}