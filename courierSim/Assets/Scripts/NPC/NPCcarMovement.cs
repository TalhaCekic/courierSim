using UnityEngine;
using System.Collections.Generic;

public class NpcCarMovement : MonoBehaviour
{
    public scribtableNpcWay scribTableNpcWay;
    public Transform Path;
    private Rigidbody rb;
    public float speed;
    public float maxSteerAngle = 50;
    private float newSteer;
    public float deleteTime;
    public bool isStop;
    public bool isFrontRayClose;
    public bool isRightRayClose;
    public bool isLeftRayClose;
    public WheelCollider wheelFl;
    public WheelCollider wheelFr;

    public List<Transform> nodes;
    public int currentNode = 0;

    public LayerMask layer;
    public LayerMask Backlayer;
    public Transform frontDetector;
    public Transform frontRightDetector;
    public Transform frontRightDetector1;
    public Transform frontLeftDetector;
    public Transform frontLeftDetector1;
    public Transform LeftDetector;
    public Transform rightDetector;

    private Ray ray;
    private Ray rayRight;
    private Ray rayLeft;
    private Ray rayRight1;
    private Ray rayLeft1;
    private Ray rayRight2;
    private Ray rayLeft2;

    public int selectWayDec;

    private void Start()
    {
        deleteTime = 20;
        if (selectWayDec == 1)
        {
            Path = GetRandomElement(ways.instace.ways1).transform;
        }
        if (selectWayDec == 2)
        {
            Path = GetRandomElement(ways.instace.ways2).transform;
        }
        if (selectWayDec == 3)
        {
            Path = GetRandomElement(ways.instace.ways3).transform;
        }
        if (selectWayDec == 4)
        {
            Path = GetRandomElement(ways.instace.ways4).transform;
        }

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

    private T GetRandomElement<T>(T[] array)
    {
        if (array != null && array.Length > 0)
        {
            int randomIndex = Random.Range(0, array.Length);
            return array[randomIndex];
        }
        else
        {
            return default(T);
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
        else
        {
            if (speed < 0)
            {
                isStop = true;
                deleteTime -= Time.deltaTime;
            }
            else
            {
                deleteTime = 20;
            }
            
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

        if (newSteer > 5)
        {
            isLeftRayClose = true;
            isRightRayClose = false;
            isFrontRayClose = true;
        }
        else if (newSteer < 5 && newSteer > -5)
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

        if (deleteTime < 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            deleteTime -= Time.deltaTime;
        }
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
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 6, Color.yellow);
        }

        if (!isRightRayClose)
        {
            rayRight = new Ray(frontRightDetector.position, frontRightDetector.forward);
            Debug.DrawLine(rayRight.origin, rayRight.origin + rayRight.direction * 4, Color.yellow);
   
            rayRight1 = new Ray(frontRightDetector1.position, frontRightDetector1.forward);
            Debug.DrawLine(rayRight1.origin, rayRight1.origin + rayRight1.direction * 4, Color.yellow);
            Debug.DrawLine(rayRight1.origin, rayRight1.origin + rayRight1.direction * 4, Color.white);
        }

        if (!isLeftRayClose)
        {
            rayLeft = new Ray(frontLeftDetector.position, frontLeftDetector.forward);
            Debug.DrawLine(rayLeft.origin, rayLeft.origin + rayLeft.direction * 4, Color.yellow);
     
            rayLeft1 = new Ray(frontLeftDetector1.position, frontLeftDetector1.forward);
            Debug.DrawLine(rayLeft1.origin, rayLeft1.origin + rayLeft1.direction * 4, Color.yellow);
            Debug.DrawLine(rayLeft1.origin, rayLeft1.origin + rayLeft1.direction * 4, Color.white);
        }

        rayLeft2 = new Ray(LeftDetector.position, LeftDetector.forward);
        Debug.DrawLine(rayLeft2.origin, rayLeft2.origin + rayLeft2.direction * 1, Color.white);
  
        rayRight2 = new Ray(rightDetector.position, rightDetector.forward);
        Debug.DrawLine(rayRight2.origin, rayRight2.origin + rayRight2.direction * 1, Color.white);
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 6, layer) || Physics.Raycast(rayRight, out hit, 4, layer) ||
            Physics.Raycast(rayLeft, out hit, 4, layer) || Physics.Raycast(rayRight1, out hit, 4, layer) ||
            Physics.Raycast(rayLeft1, out hit, 4, layer) || Physics.Raycast(rayRight, out hit, 4, Backlayer) ||
            Physics.Raycast(rayLeft, out hit, 4, Backlayer) ||
            Physics.Raycast(rayLeft1, out hit, 4, Backlayer) || Physics.Raycast(rayRight1, out hit, 4, Backlayer) ||
            Physics.Raycast(rayLeft1, out hit, 4, Backlayer) || Physics.Raycast(rayRight2, out hit, 1, Backlayer) ||
            Physics.Raycast(rayLeft2, out hit, 1, Backlayer))
        {
            if (hit.transform == this.transform)
            {
                isStop = false;
            }
            else
            {
                isStop = true;
            }
        }
        else
        {
            isStop = false;
        }
    }
}