using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class interact : MonoBehaviour
{
    [SerializeField] public int maxDistance;
    private PlayerInput playerInput;

     public Image interactImage; 
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        maxDistance = 1;

        playerInput.currentActionMap["interact"].Enable();
        playerInput.currentActionMap["interact"].performed += Interact;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

       
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            interactImage.color = Color.green;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
       
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

       
        RaycastHit hit;

       
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
           
            Debug.Log("İşlenen Nesne: " + hit.transform.name);

           
            hit.transform.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}