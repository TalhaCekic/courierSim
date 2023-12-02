using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class cam : MonoBehaviour
{   
    public float sensitivity = 2.0f;

    private void Update()
    {
        rotate();
    }

    void rotate()
    {
        // Hareket Kontrolleri
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        
        // Fare Hareketi ile Kamera Dönme
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 rotation = new Vector3(-mouseY, mouseX, 0) * sensitivity;
        transform.Rotate(rotation);
    }
}
