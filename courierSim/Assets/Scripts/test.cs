using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Transform target;  
    public Vector3 offset = new Vector3(0f, 3f, -7f);  

    [Range(0.1f, 10.0f)]
    public float rotationSpeed = 5.0f;
    [Range(0.1f, 10.0f)]
    public float followSpeed = 5.0f;

    private void LateUpdate()
    {
        if (interact.instance.isMotor)
        {
            if (target == null)
            {
                Debug.LogWarning("Araç atanmamış!");
                return;
            }

            // Aracın etrafında dönme
            float horizontalInput = Input.GetAxis("Horizontal");
            float desiredRotationAngle = target.eulerAngles.y + horizontalInput * rotationSpeed;
            Quaternion rotation = Quaternion.Euler(0, desiredRotationAngle, 0);
            transform.position = Vector3.Lerp(transform.position, target.position - (rotation * offset),
                followSpeed * Time.deltaTime);

            // Kameranın araca bakmasını sağlama
            transform.LookAt(target); 
        }
    
    }
}