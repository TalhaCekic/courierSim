using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;

public class phoneMenu : MonoBehaviour
{
    public static phoneMenu instance;
    private PlayerInput playerInput;
    public GameObject phoneCanvas;
    public GameObject subBarButtonObj;
    public bool isSubBar;
    public GameObject map;
    public bool isPhoneActive;
    public bool isMapActive;

    public bool isNotification;
    public GameObject notification;
    public Image notificationBackground;
    public TMP_Text notificationText;

    public Button GoButton;

    float phoneLerpSpeed = 10f;

    private GameObject mapCam;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mapCam = GameObject.FindGameObjectWithTag("mapCamera");
    }

    void Start()
    {
        instance = this;
        isSubBar = false;
        phoneCanvas.transform.localPosition = new Vector3(0, -850, 0);

        playerInput.currentActionMap["phone"].Enable();
        playerInput.currentActionMap["phone"].performed += PhoneButton;
    }

    private void Update()
    {
        Notification();
        phone();

        if (!OrderManager.instance.isSearchingOrder && !OrderManager.instance.isOrder)
        {
            GoButton.gameObject.SetActive(true);
        }

        mapCam.transform.position = new Vector3(this.transform.position.x,35,this.transform.position.z);
    }
    
    // telefon etkileşimi
    private void phone()
    {
        if (isPhoneActive)
        {
            phoneCanvas.transform.localPosition = Vector3.Lerp(phoneCanvas.transform.localPosition,
                new Vector3(0, 0, 0), phoneLerpSpeed * Time.deltaTime);

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
            if (isNotification)
            {
                phoneCanvas.transform.localPosition = Vector3.Lerp(phoneCanvas.transform.localPosition,
                    new Vector3(0, -600, 0), phoneLerpSpeed * Time.deltaTime);
            }
            else
            {
                phoneCanvas.transform.localPosition = Vector3.Lerp(phoneCanvas.transform.localPosition,
                    new Vector3(0, -850, 0), phoneLerpSpeed * Time.deltaTime);
            }

            Cursor.lockState = CursorLockMode.Locked;
        }
        if (isSubBar)
        {
            subBarButtonObj.transform.localPosition = Vector3.Lerp(subBarButtonObj.transform.localPosition,
                new Vector3(0, 0, 0), phoneLerpSpeed * Time.deltaTime);
        }
        else
        {
            subBarButtonObj.transform.localPosition = Vector3.Lerp(subBarButtonObj.transform.localPosition,
                new Vector3(0, -250, 0), phoneLerpSpeed * Time.deltaTime);
        }
    }
    // notification ayarları 
    private void Notification()
    {
        if (isNotification)
        {
            notification.transform.localPosition = Vector3.Lerp(notification.transform.localPosition,
                new Vector3(0, 0, 0), phoneLerpSpeed * Time.deltaTime);
        }
        else
        {
            notification.transform.localPosition = Vector3.Lerp(notification.transform.localPosition,
                new Vector3(0, 90, 0), phoneLerpSpeed * Time.deltaTime);
        }

        if (OrderManager.instance.isOrderFound)
        {
            notificationBackground.color = Color.green;
            notificationText.text = " sipariş bulundu ";
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

    public void SubBarButton()
    {
        if (OrderManager.instance.isOrder)
        {
            isSubBar = !isSubBar;
        }
    }

    public void SearchForOrder()
    {
        GoButton.gameObject.SetActive(false);
        OrderManager.instance.isSearchingOrder = true;
        notificationText.text = " Sipariş aranıyor...";
        isNotification = true;
    }

    public void a()
    {
        //map.SetActive(true);
        isMapActive = true;
    }

    public void back()
    {
        map.SetActive(false);
        isMapActive = false;
    }
}