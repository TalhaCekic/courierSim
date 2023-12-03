using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class interact : MonoBehaviour
{
    public static interact instance;
    [SerializeField] public int maxDistance;
    private PlayerInput playerInput;

     public Image interactImage; 
     public bool isMotor;
     
     public LayerMask CarLayer;
     private CapsuleCollider cap;
     private GameObject obj;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        instance = this;
        isMotor = false;
        cap = GetComponent<CapsuleCollider>();
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
        else
        {
             interactImage.color = Color.black;
        }

        if (isMotor)
        {
            this.transform.transform.position = obj.transform.position;
            this.transform.transform.rotation = obj.transform.rotation;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance,CarLayer))
        {
           
            Debug.Log("İşlenen Nesne: " + hit.transform.name);
            cap.enabled = false;
            obj = hit.transform.gameObject;
            this.transform.SetParent(hit.transform);
            this.transform.position = hit.transform.position;

            isMotor = !isMotor;
        }
    }
}