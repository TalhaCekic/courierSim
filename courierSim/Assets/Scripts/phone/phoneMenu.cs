using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class phoneMenu : MonoBehaviour
{
    public static phoneMenu instance;
    private PlayerInput playerInput;
    public GameObject phoneCanvas;
    public GameObject map;
    public bool isPhoneActive;
    public bool isMapActive;
    
    float phoneLerpSpeed = 10f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        instance = this;
        phoneCanvas.transform.localPosition = new Vector3(0, -850, 0);
        
        playerInput.currentActionMap["phone"].Enable();
        playerInput.currentActionMap["phone"].performed += PhoneButton;
    }

    private void Update()
    {
        if (isPhoneActive)
        {
            phoneCanvas.transform.localPosition = Vector3.Lerp(phoneCanvas.transform.localPosition, new Vector3(0,0,0), phoneLerpSpeed * Time.deltaTime);
            if (isMapActive)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
        else
        {
            phoneCanvas.transform.localPosition = Vector3.Lerp(phoneCanvas.transform.localPosition, new Vector3(0,-850,0), phoneLerpSpeed * Time.deltaTime);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void PhoneButton(InputAction.CallbackContext context)
    {
        if (!isMapActive)
        {
            isPhoneActive = !isPhoneActive;  
        }
        else
        {
            back();
        }
    
    }

    public void a()
    {
        map.SetActive(true);
        isMapActive = true;
    }

    public void back()
    {
        map.SetActive(false);
        isMapActive = false;
    }
}