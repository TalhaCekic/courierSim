using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    private GameObject altObje;
    private GameObject altObje2;

    public TMP_Text speedText;

    float resetDelay = 0.5f;
    public float lastResetTime = -1f;

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
            this.transform.transform.position = altObje2.transform.position;
            this.transform.transform.rotation = altObje2.transform.rotation;
            interactImage.gameObject.SetActive(false);
            speedText.gameObject.SetActive(true);
        }
        else
        {
            interactImage.gameObject.SetActive(true);
            speedText.gameObject.SetActive(false);
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Time.time - lastResetTime > resetDelay)
        {
            if (isMotor)
            {
                obj = null;
                isMotor = false;
                cap.enabled = true;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (Physics.Raycast(ray, out hit, maxDistance, CarLayer))
            {
                cap.enabled = false;
                obj = hit.transform.gameObject;
                altObje = obj.transform.Find("Plane").gameObject;
                altObje2 = altObje.transform.Find("stay").gameObject;
                //altObje = hit.transform.gameObject;
                this.transform.SetParent(hit.transform);
                this.transform.position = hit.transform.position;

                isMotor = !isMotor;
            }
        }

     
    }
}