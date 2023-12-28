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
    private spawnOrderObj orderObj;
    public scribtableOrders ScribtableOrders;
    [SerializeField] public int maxDistance;
    private PlayerInput playerInput;

    public Image interactImage;
    public bool isMotor;
    public bool isBoxOpen;
    public bool isChangeCameraPov;

    public bool isHasBurger;
    public bool isHasPizza;

    public LayerMask CarLayer;
    public LayerMask CarBoxLayer;
    public LayerMask BurgerShop;
    public LayerMask PizzaShop;
    public LayerMask DeliveryPosition;
    private CapsuleCollider cap;
    private GameObject obj;
    private GameObject altObje;
    private GameObject altObje2;

    public TMP_Text speedText;

    float resetDelay = 0.5f;
    public float lastResetTime = -1f;

    public GameObject[] orders;
    public Transform hand;

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

        playerInput.currentActionMap["cameraChange"].Enable();
        playerInput.currentActionMap["cameraChange"].performed += CameraChange;
    }

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
            if (!isChangeCameraPov)
            {
                Camera.main.transform.SetParent(obj.transform);
            }
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

            //order detayları
            orders = GameObject.FindGameObjectsWithTag("Order");
            //eğer sipariş varsa
            if (OrderManager.instance.isOrderStart)
            {
                if (Physics.Raycast(ray, out hit, maxDistance, BurgerShop))
                {
                    if (OrderManager.instance.isBurger && OrderManager.instance.isOrder)
                    {
                        isHasBurger = true;
                        Instantiate(ScribtableOrders.BurgerOrderPrefabObj,hand.position,hand.rotation);
                    }
                }

                if (Physics.Raycast(ray, out hit, maxDistance, PizzaShop))
                {
                    if (OrderManager.instance.isPizza && OrderManager.instance.isOrder)
                    {
                        isHasPizza = true;
                        Instantiate(ScribtableOrders.PizzaOrderPrefabObj,hand.position,hand.rotation);
                    }
                }

                if (Physics.Raycast(ray, out hit, maxDistance, DeliveryPosition))
                {
                    if (OrderManager.instance.isPizza ||
                        OrderManager.instance.isBurger && OrderManager.instance.isOrder)
                    {
                        if (OrderManager.instance.selectedDeliveryPosition.name == hit.transform.transform.name)
                        {
                            DeleteFirstChild(hit.transform);
                            for (int i = 0; i < orders.Length; i++)
                            {
                                if (orders[i] != null)
                                {
                                    Destroy(orders[i]);
                                    orders[i] = null;
                                }
                            }

                            OrderManager.instance.isOrderStart = false;
                            OrderManager.instance.isOrder = false;
                            OrderManager.instance.isSpawn = false;
                            OrderManager.instance.isBurger = false;
                            OrderManager.instance.isPizza = false;
                            OrderManager.instance.isSearchingOrder = false;
                            OrderManager.instance.isdelivery = true;
                        }
                    }
                }
            }

            if (Physics.Raycast(ray, out hit, maxDistance, CarBoxLayer))
            {
                isBoxOpen = !isBoxOpen;
            }
            else if (Physics.Raycast(ray, out hit, maxDistance, CarLayer))
            {
                cap.enabled = false;
                obj = hit.transform.gameObject;
                altObje = obj.transform.Find("Plane").gameObject;
                altObje2 = altObje.transform.Find("stay").gameObject;
                this.transform.SetParent(hit.transform);
                this.transform.position = hit.transform.position;
                isMotor = !isMotor;
            }
        }
    }
    void DeleteFirstChild(Transform parent)
    {
        if (parent.childCount > 0)
        {
            Destroy(parent.GetChild(0).gameObject);
        }
        else
        {
            Debug.LogWarning("No child object to delete.");
        }
    }
    public void itemInteract()
    {
    }

    public void CameraChange(InputAction.CallbackContext context)
    {
        isChangeCameraPov = !isChangeCameraPov;
    }
}