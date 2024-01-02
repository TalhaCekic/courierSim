using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Image = UnityEngine.UI.Image;

public class interact : MonoBehaviour
{
    public static interact instance;
    private Animator anims;
    private spawnOrderObj orderObj;
    public scribtableOrders ScribtableOrders;
    [SerializeField] public int maxDistance;
    private PlayerInput playerInput;

    public Image interactImage;
    public bool isMotor;
    public bool isBoxOpen;
    public bool isChangeCameraPov;

    public GameObject isHasOrder;
    public bool isHasBurger;
    public bool isBurgerYes;
    public bool isHasPizza;
    public bool isPizzaYes;

    public LayerMask layers;
    public LayerMask Orderlayers;
    public LayerMask CarLayer;
    public LayerMask CarBoxLayer;
    public LayerMask BurgerShop;
    public LayerMask PizzaShop;
    public LayerMask DeliveryPosition;
    public LayerMask tresh;
    private CapsuleCollider cap;
    private GameObject obj;
    private GameObject altObje;
    private GameObject altObje2;

    public TMP_Text speedText;

    float resetDelay = 0.5f;
    public float lastResetTime = -1f;

    public GameObject[] orders;
    public Transform hand;
    public Transform putPosition;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        instance = this;
        isMotor = false;
        cap = GetComponent<CapsuleCollider>();
        anims = GetComponent<Animator>();
    }

    void Start()
    {
        maxDistance = 1;

        playerInput.currentActionMap["interact"].Enable();
        playerInput.currentActionMap["interact"].performed += Interact;
        playerInput.currentActionMap["interact2"].Enable();
        playerInput.currentActionMap["interact2"].performed += Interact2;

        playerInput.currentActionMap["cameraChange"].Enable();
        playerInput.currentActionMap["cameraChange"].performed += CameraChange;
    }

    void Update()
    {
        // imleç renkleri ayarı
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 2, Color.red);

        if (Physics.Raycast(ray, out hit, maxDistance, layers))
        {
            interactImage.color = Color.green;
        }
        else if (Physics.Raycast(ray, out hit, maxDistance, Orderlayers) && !isHasPizza && !isHasBurger)
        {
            interactImage.color = Color.white;
        }
        else
        {
            interactImage.color = Color.black;
        }

        // motor binince kamera eşitlemesi
        if (isMotor)
        {
            this.transform.transform.position = altObje2.transform.position;
            this.transform.transform.rotation = altObje2.transform.rotation;
            interactImage.gameObject.SetActive(false);
            speedText.gameObject.SetActive(true);
            if (!isChangeCameraPov)
            {
                Camera.main.transform.SetParent(altObje2.transform);
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
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            //order detayları
            orders = GameObject.FindGameObjectsWithTag("Order");

            //eğer sipariş varsa gidip almak için
            if (OrderManager.instance.isOrderStart)
            {
                if (Physics.Raycast(ray, out hit, maxDistance, BurgerShop))
                {
                    if (OrderManager.instance.isBurger && OrderManager.instance.isOrder && !isHasPizza && !isHasBurger && !isBurgerYes)
                    {
                        isHasBurger = true;
                        isBurgerYes = true;
                        Instantiate(ScribtableOrders.BurgerOrderPrefabObj, hand.position, hand.rotation, hand);
                        isHasOrder = hand.GetChild(0).gameObject;
                        anims.SetBool("hold", isHasBurger);

                        // eldeki obje sayısını ayarlama
                        int handCount = hand.childCount;
                        for (handCount = 0; handCount > 1; handCount++)
                        {
                            Destroy(hand.GetChild(handCount));
                            print("silme işlemi yap");
                        }
                    }
                }

                if (Physics.Raycast(ray, out hit, maxDistance, PizzaShop))
                {
                    if (OrderManager.instance.isPizza && OrderManager.instance.isOrder && !isHasPizza && !isHasBurger && !isPizzaYes)
                    {
                        isHasPizza = true;
                        isPizzaYes = true;
                        Instantiate(ScribtableOrders.PizzaOrderPrefabObj, hand.position, hand.rotation, hand);
                        isHasOrder = hand.GetChild(0).gameObject;
                        anims.SetBool("hold", isHasPizza);
                        
                        // eldeki obje sayısını ayarlama
                        int handCount = hand.childCount;
                        for (handCount = 0; handCount > 1; handCount++)
                        {
                            Destroy(hand.GetChild(handCount));
                        }
                    }
                }

                if (Physics.Raycast(ray, out hit, maxDistance, DeliveryPosition))
                {
                    if (OrderManager.instance.isPizza ||
                        OrderManager.instance.isBurger && OrderManager.instance.isOrder && isHasBurger || isHasPizza)
                    {
                        if (OrderManager.instance.selectedDeliveryPosition.name == hit.transform.transform.name)
                        {
                            DeleteFirstChild(hit.transform);
                            for (int i = 0; i < orders.Length; i++)
                            {
                                if (orders[i] != null)
                                {
                                    Destroy(hand.GetChild(0).gameObject);
                                    Destroy(orders[i]);
                                    isHasOrder = null;
                                    
                                    print(hand.GetChild(0));
                                    orders[i] = null;
                                    anims.SetBool("hold", false);
                                    isBurgerYes = false;
                                    isPizzaYes = false;
                                    isHasPizza = false;
                                    isHasBurger = false;
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

            // motor arkasındaki kutuya gönderilen işlem
            if (Physics.Raycast(ray, out hit, maxDistance, CarBoxLayer))
            {
                if (isBoxOpen)
                {
                    putPosition = hit.transform.GetChild(0);
                    motorPut MotorPut = hit.transform.GetComponent<motorPut>();
                    if (isHasOrder == null)
                    {
                        putPosition.GetChild(0).transform.SetParent(hand.transform);
                        isHasOrder = hand.GetChild(0).gameObject;
                        isHasOrder.transform.position = hand.transform.position;
                        isHasOrder.transform.rotation = hand.transform.rotation;
                        isHasOrder.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        anims.SetBool("hold", true);

                        if (MotorPut.isBurger)
                        {
                            isHasBurger = true;
                            MotorPut.isBurger = false;
                        }

                        if (MotorPut.isPizza)
                        {
                            isHasPizza = true;
                            MotorPut.isPizza = false;
                        }
                    }
                    else if (isHasOrder != null)
                    {
                        isHasOrder.transform.SetParent(putPosition);
                        isHasOrder.transform.position = putPosition.position;
                        isHasOrder.transform.rotation = putPosition.rotation;
                        isHasOrder.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        if (isHasPizza)
                        {
                            MotorPut.isPizza = true;
                            isHasPizza = false;
                        }

                        if (isHasBurger)
                        {
                            MotorPut.isBurger = true;
                            isHasBurger = false;
                        }

                        anims.SetBool("hold", false);
                        isHasOrder = null;
                    }
                }
            }

            // if (Physics.Raycast(ray, out hit, maxDistance, CarBoxLayer))
            // {
            //     
            //     if (isHasOrder != null)
            //     {
            //         motorPut MotorPut = hit.transform.GetComponent<motorPut>();
            //         if (MotorPut.isBurger)
            //         {
            //             Instantiate(ScribtableOrders.BurgerOrderPrefabObj, hand.position, hand.rotation, hand);
            //         }
            //     
            //         if (MotorPut.isPizza)
            //         {
            //             Instantiate(ScribtableOrders.PizzaOrderPrefabObj, hand.position, hand.rotation, hand);
            //         } 
            //     }
            // }
            else if (Physics.Raycast(ray, out hit, maxDistance, CarLayer))
            {
                if (!isHasPizza || !isHasBurger)
                {
                    obj = hit.transform.gameObject;
                    //altObje = obj.transform.Find("Plane").gameObject;
                    altObje = hit.transform.gameObject;
                    altObje2 = altObje.transform.Find("stay").gameObject;
                    this.transform.SetParent(hit.transform);
                    this.transform.position = hit.transform.position;
                    isMotor = !isMotor;
                }
            }
        }
    }

    public void Interact2(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, CarBoxLayer))
        {
            isBoxOpen = !isBoxOpen;
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

    public void CameraChange(InputAction.CallbackContext context)
    {
        isChangeCameraPov = !isChangeCameraPov;
    }
}