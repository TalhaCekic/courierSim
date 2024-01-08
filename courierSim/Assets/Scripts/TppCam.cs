using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TppCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3f, -7f);

    public float rotationSpeed = 5.0f;
    //public float followSpeed = 5.0f;

    public float sensitivity = 2.0f; // Fare hassasiyeti
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    public float restTime;
    public bool isReset;
    public bool currentReset;

    public float lerpSpeed;

    float mouseX;
    float mouseY;
    private Quaternion rotation;

    private void FixedUpdate()
    {
        if (interact.instance.isMotor)
        {
            mouseX = Input.GetAxis("Mouse X") * sensitivity;
            mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            if (mouseX == 0 && isReset || mouseY == 0 && isReset)
            {
                if (target == null)
                {
                    Debug.LogWarning("Araç atanmamış!");
                    return;
                }

                float horizontalInput = Input.GetAxis("Horizontal");
                transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);

                // Aracın pozisyonunu takip etme
                Vector3 targetPosition = target.position + target.TransformDirection(offset);
                transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
                
                transform.LookAt(target);
                if (mouseX < 0 || mouseX > 0 && mouseY < 0 || mouseY > 0)
                {
                    isReset = false;
                }
            }
            else if (!isReset)
            {
                currentX += mouseX;
                currentY += mouseY;
                rotation = Quaternion.Euler(currentY, currentX, 0);
                Vector3 desiredPosition = target.position - (rotation * Vector3.forward + (rotation * offset));

                transform.position = Vector3.Lerp(transform.position,  desiredPosition , 1);
                //transform.position = desiredPosition;

                transform.LookAt(target);
                currentReset = true;

                // süreyi sıfırla
                if (mouseX < 0 || mouseX > 0 && mouseY < 0 || mouseY > 0)
                {
                    restTime = 5;
                }
            }
            else
            {
                isReset = false;
                currentReset = true;
            }
        }
    }

    private void Update()
    {
        if (interact.instance.isMotor)
        {
            //reset hesapları
            if (currentReset)
            {
                isReset = false;
                restTime -= Time.deltaTime;
                if (restTime < 0)
                {
                    isReset = true;
                    currentReset = false;
                }
            }
            else
            {
                restTime = 5;
                isReset = true;
            }
        }
    }
}