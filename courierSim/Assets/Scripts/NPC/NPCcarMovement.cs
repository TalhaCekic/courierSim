using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.Serialization;

public class NPCcarMovement : MonoBehaviour
{
    public Transform Path;
    private Rigidbody rb;
    public float speed;
    public float maxSteerAngle = 50;
    private float newSteer;
    public bool isStop;
    public bool isFrontRayClose;
    public bool isRightRayClose;
    public bool isLeftRayClose;
    public WheelCollider wheelFl;
    public WheelCollider wheelFr;
    public GameObject wheelFlObj;
    public GameObject wheelFrObj;

    public List<Transform> nodes;
    public int currentNode = 0;

    public LayerMask layer;
    public LayerMask Backlayer;
    public Transform frontDetector;
    public Transform frontRightDetector;
    public Transform frontLeftDetector;

    private Ray ray;
    private Ray rayRight;
    private Ray rayLeft;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        speed = rb.velocity.magnitude * 3.6f;
        ApplySteer();
        CheckWayPointDistance();
        detector();
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 10)
        {
            if (speed > 15 || isStop)
            {
                stop();
            }
            else
            {
                slowDrive();
            }
        }
        else if (speed < 20)
        {
            Drive();
        }

        if (isStop)
        {
            stop();
        }
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFl.steerAngle = newSteer;
        wheelFr.steerAngle = newSteer;
        if (newSteer < -5)
        {
            isRightRayClose = true;
            isLeftRayClose = false;
            isFrontRayClose = true;
        }     
        if (newSteer >5)
        {
            isLeftRayClose = true;
            isRightRayClose = false;
            isFrontRayClose = true;
        }
        else if(newSteer <5 && newSteer > -5)
        {
            isFrontRayClose = false;
            isLeftRayClose = false;
            isRightRayClose = false;
        }
    }

    private void Drive()
    {
        wheelFl.motorTorque = 30;
        wheelFr.motorTorque = 30;
        wheelFl.brakeTorque = 0;
        wheelFr.brakeTorque = 0;
    }

    private void slowDrive()
    {
        wheelFl.motorTorque = 15;
        wheelFr.motorTorque = 15;
        wheelFl.brakeTorque = 0;
        wheelFr.brakeTorque = 0;
    }

    private void stop()
    {
        wheelFl.motorTorque = 0;
        wheelFr.motorTorque = 0;
        wheelFl.brakeTorque = 5000;
        wheelFr.brakeTorque = 5000;
    }

    private void CheckWayPointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 2f)
        {
            currentNode++;
            if (currentNode >= nodes.Count)
            {
                currentNode = 0;
            }
        }
    }

    private void detector()
    {
        if (!isFrontRayClose)
        {
           ray = new Ray(frontDetector.position, frontDetector.forward); 
           Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10, Color.yellow);
        }

        if (!isRightRayClose)
        {
            rayRight = new Ray(frontRightDetector.position, frontRightDetector.forward);
            Debug.DrawLine(rayRight.origin, rayRight.origin + rayRight.direction * 5, Color.yellow);
        }

        if (!isLeftRayClose)
        {
            rayLeft = new Ray(frontLeftDetector.position, frontLeftDetector.forward);
            Debug.DrawLine(rayLeft.origin, rayLeft.origin + rayLeft.direction * 5, Color.yellow);
        }
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10, layer) || Physics.Raycast(rayRight, out hit, 5, layer)|| Physics.Raycast(rayLeft, out hit, 5, layer))
        {
            isStop = true;
        }
        else
        {
            isStop = false;
        }
    }
}