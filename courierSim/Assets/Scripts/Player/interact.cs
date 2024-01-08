using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class interact : MonoBehaviour
{
    public static interact instance;
    private Animator anims;
    private spawnOrderObj orderObj;
    public scribtableOrders ScribtableOrders;
    [SerializeField] public int maxDistance;
    private PlayerInput playerInput;

    public TMP_Text KeyInputText;

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
    public LayerMask trash;
    private CapsuleCollider cap;
    private GameObject obj;
    private GameObject altObje;
    private GameObject altObje2;

    public TMP_Text speedText;
    public Slider speedSlider;

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
        keyInteract();
        // motor binince kamera eşitlemesi
        if (isMotor)
        {
            this.transform.transform.position = altObje2.transform.position;
            this.transform.transform.rotation = altObje2.transform.rotation;
            interactImage.gameObject.SetActive(false);
            speedText.gameObject.SetActive(true);
            speedSlider.gameObject.SetActive(true);
            if (isChangeCameraPov)
            {
                Camera.main.transform.SetParent(altObje2.transform);
            }
        }
        else
        {
            interactImage.gameObject.SetActive(true);
            speedText.gameObject.SetActive(false);
            speedSlider.gameObject.SetActive(false);
        }

        if (isHasBurger || isHasPizza)
        {
            anims.SetBool("hold", true);
        }
        else
        {
            anims.SetBool("hold", false);
            ;
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
            else
            {
                interact.instance.isChangeCameraPov = false;
            }

            //order detayları
            orders = GameObject.FindGameObjectsWithTag("Order");

            //eğer sipariş varsa gidip almak için
            if (OrderManager.instance.isOrderStart)
            {
                if (Physics.Raycast(ray, out hit, maxDistance, BurgerShop))
                {
                    if (OrderManager.instance.isBurger && OrderManager.instance.isOrder && !isBurgerYes)
                    {
                        Instantiate(ScribtableOrders.BurgerOrderPrefabObj, hand);
                        isHasOrder = hand.GetChild(0).gameObject;
                        isHasOrder.transform.localPosition = new Vector3(-0.361f, 0.099f, 0.014f);
                        isHasOrder.transform.localRotation = Quaternion.Euler(-30.133f, -83.883f, 141.323f);
                        isHasBurger = true;
                        isBurgerYes = true;
                    }
                }

                if (Physics.Raycast(ray, out hit, maxDistance, PizzaShop))
                {
                    if (OrderManager.instance.isPizza && OrderManager.instance.isOrder && !isPizzaYes)
                    {
                        Instantiate(ScribtableOrders.PizzaOrderPrefabObj, hand.position, hand.rotation, hand);
                        isHasOrder = hand.GetChild(0).gameObject;
                        //isHasOrder.transform.localPosition = new Vector3(-0.361f, 0.099f, 0.014f);
                        //isHasOrder.transform.localRotation = Quaternion.Euler(-30.133f, -83.883f, 141.323f);
                        isHasPizza = true;
                        isPizzaYes = true;
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
                                    // teslim etme sonraso para kazanma
                                    phoneMenu.instance.price += spawnOrderObj.instance.OrderPrice;
                                    phoneMenu.instance.isTruePay = true;
                                    Destroy(hand.GetChild(0).gameObject);
                                    Destroy(orders[i]);
                                    isHasOrder = null;

                                    orders[i] = null;
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
                        isHasOrder.transform.localPosition = new Vector3(-0.361f, 0.099f, 0.014f);
                        isHasOrder.transform.localRotation = Quaternion.Euler(-30.133f, -83.883f, 141.323f);
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
                        isHasOrder.transform.localPosition = new Vector3(0, 0, 0.1f);
                        isHasOrder.transform.localRotation = Quaternion.Euler(0, 0, 180);
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
            else if (Physics.Raycast(ray, out hit, maxDistance, CarLayer))
            {
                if (!isHasPizza || !isHasBurger)
                {
                    obj = hit.transform.gameObject;
                    altObje = hit.transform.gameObject;
                    altObje2 = altObje.transform.Find("stay").gameObject;
                    this.transform.SetParent(hit.transform);
                    this.transform.position = hit.transform.position;
                    this.transform.rotation = hit.transform.rotation;
                    isMotor = !isMotor;
                }
            }

            if (Physics.Raycast(ray, out hit, maxDistance, trash))
            {
                Destroyer();
                //     DeleteFirstChild(hit.transform);
                //     Destroy(hand.GetChild(0).gameObject);
                //     for (int i = 0; i < orders.Length; i++)
                //     {
                //         if (orders[i] != null)
                //         {
                //             
                //             Destroy(orders[i]);
                //             isHasOrder = null;
                //
                //             orders[i] = null;
                //             isBurgerYes = false;
                //             isPizzaYes = false;
                //             isHasPizza = false;
                //             isHasBurger = false;
                //         }
                //     }
                //
                //     OrderManager.instance.isOrderStart = false;
                //     OrderManager.instance.isOrder = false;
                //     OrderManager.instance.isSpawn = false;
                //     OrderManager.instance.isBurger = false;
                //     OrderManager.instance.isPizza = false;
                //     OrderManager.instance.isSearchingOrder = false;
                //     OrderManager.instance.isdelivery = true;
                // }
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
        if (isMotor)
        {
            isChangeCameraPov = !isChangeCameraPov;
        }
    }

    public void Destroyer()
    {
        //DeleteFirstChild(hit.transform);
        if (isHasBurger || isHasPizza)
        {
            Destroy(hand.GetChild(0).gameObject);
        }

        for (int i = 0; i < orders.Length; i++)
        {
            if (orders[i] != null)
            {
                Destroy(orders[i]);
                isHasOrder = null;

                orders[i] = null;
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


    // İNPUT TEXTE YAZDIRMAK İÇİN:
    private void keyInteract()
    {
        // imleç renkleri ayarı
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 2, Color.red);

        if (Physics.Raycast(ray, out hit, maxDistance, layers))
        {
            interactImage.color = Color.green;
        }
        else
        {
            interactImage.color = Color.black;
        }

        if (OrderManager.instance.isOrderStart)
        {
            if (Physics.Raycast(ray, out hit, maxDistance, BurgerShop) && !isBurgerYes && !isHasBurger ||
                Physics.Raycast(ray, out hit, maxDistance, PizzaShop) && !isBurgerYes && !isHasPizza)
            {
                KeyInputText.text = " Siparişi almak için 'E' Tuşuna Bas. ";
                KeyInputText.color = Color.white;
                interactImage.color = Color.white;
            }

            if (Physics.Raycast(ray, out hit, maxDistance, DeliveryPosition))
            {
                if (OrderManager.instance.selectedDeliveryPosition.name == hit.transform.transform.name)
                {
                    KeyInputText.text = " Siparişi Teslim etmek için 'E' Tuşuna Bas. ";
                    KeyInputText.color = Color.green;
                    interactImage.color = Color.green;
                }
                else
                {
                    KeyInputText.text = " Yanlış Adres ";
                    KeyInputText.color = Color.red;
                    interactImage.color = Color.red;
                }
            }

            if (Physics.Raycast(ray, out hit, maxDistance, CarBoxLayer))
            {
                KeyInputText.text = " Sepeti Açıp Kapatmak için 'F' Tuşuna Bas. ";
                KeyInputText.color = Color.white;
                interactImage.color = Color.white;
            }
            else if (Physics.Raycast(ray, out hit, maxDistance, CarLayer))
            {
                KeyInputText.text = " Motora Binmek için 'E' Tuşuna Bas. ";
                KeyInputText.color = Color.white;
                interactImage.color = Color.white;
            }
            else if (!Physics.Raycast(ray, out hit, maxDistance, CarLayer) &&
                     !Physics.Raycast(ray, out hit, maxDistance, CarBoxLayer) &&
                     !Physics.Raycast(ray, out hit, maxDistance, DeliveryPosition) &&
                     !Physics.Raycast(ray, out hit, maxDistance, BurgerShop) &&
                     !Physics.Raycast(ray, out hit, maxDistance, PizzaShop))
            {
                KeyInputText.text = " ";
            }
        }
        else
        {
            if (Physics.Raycast(ray, out hit, maxDistance, CarBoxLayer))
            {
                KeyInputText.text = " Sepeti Açıp Kapatmak için 'F' Tuşuna Bas. ";
                KeyInputText.color = Color.white;
                interactImage.color = Color.white;
            }
            else if (Physics.Raycast(ray, out hit, maxDistance, CarLayer))
            {
                KeyInputText.text = " Motora Binmek için 'E' Tuşuna Bas. ";
                KeyInputText.color = Color.white;
                interactImage.color = Color.white;
            }
            else
            {
                KeyInputText.text = " ";
            }
        }
    }
}