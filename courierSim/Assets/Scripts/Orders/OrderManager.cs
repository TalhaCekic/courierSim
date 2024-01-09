using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;

    public scribtableOrders scribtableOrders;

    public string Order;
    public string orderName;
    public int orderPrice;
    public int orderPriceIndex;
    public int tipPrice;
    public int orderTime;
    public GameObject[] BurgerPositions;
    public GameObject[] PizzaPositions;
    public GameObject[] DeliveryPositions;

    public GameObject selectedBurgerPosition;
    public GameObject selectedPizzaPosition;
    public GameObject selectedDeliveryPosition;

    private float minInterval = 10f; //10
    private float maxInterval = 20f; //20
    public float nextOrderTime = 0f;
    private float delay=3;

    public bool isSearchingOrder;
    public bool isOrderFound;
    public bool isOrderStart;
    public bool isSpawn;
    public bool isOrder;
    public bool isBurger;
    public bool isPizza;
    public bool isdelivery;

    void Start()
    {
        instance = this;
        isSpawn = false;
        BurgerPositions = GameObject.FindGameObjectsWithTag("BurgerShop");

        PizzaPositions = GameObject.FindGameObjectsWithTag("PizzaShop");
        DeliveryPositions = GameObject.FindGameObjectsWithTag("DeliveryPosition");
        selectedBurgerPosition = BurgerPositions[0];
        selectedPizzaPosition = PizzaPositions[0];
        timeOrderSpawn();
    }

    void Update()
    {
        if (Time.time >= nextOrderTime && !isOrder && isSearchingOrder)
        {
            timeOrderSpawn();
            
        }

        if (isOrderFound)
        {
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                delay = 0;
                spawnOrderPosition();
            }
        }
        if(!isOrder)
        {
            DeleteFirstChild(selectedDeliveryPosition.transform);
            Order = null;
            orderName = null;
            selectedDeliveryPosition = null;
        }
        if (!isOrder && isdelivery)
        {
            isBurger = false;
            isPizza = false;
            isSpawn = false;
            isOrderStart = false;
            isSearchingOrder = false;
            delay = 3;
        }
    }

    public void timeOrderSpawn()
    {
        nextOrderTime = Time.time + Random.Range(minInterval, maxInterval);
        if (isSearchingOrder)
        {
            isOrderFound = true;  
        }
    }
    public void spawnOrderPosition()
    {
        Order = GetRandomElement(scribtableOrders.order);
        orderName = GetRandomElement(scribtableOrders.CustomerNames);
        orderPrice = GetRandomElement(scribtableOrders.orderPrice);
        tipPrice = GetRandomElement(scribtableOrders.tipPrice);
        orderTime = GetRandomElement(scribtableOrders.orderTimes);
        orderPriceIndex = GetIndexInArray(orderPrice, scribtableOrders.orderPrice);
        selectedDeliveryPosition = GetRandomElement(DeliveryPositions);
        isOrder = true;
        isSpawn = true;
        isSearchingOrder = false;
        isOrderFound = false;
        delay = 3;
        phoneMenu.instance.isNotification = false;
        phoneMenu.instance.isSubBar = true;
        
        Instantiate(scribtableOrders.deliverySelectedIU, new Vector3(selectedDeliveryPosition.transform.position.x,30,selectedDeliveryPosition.transform.position.z),
            Quaternion.Euler(90,0,90), selectedDeliveryPosition.transform);
        if (Order == "Burger")
        {
            isBurger = true;
        }
        if (Order == "Pizza")
        {
            isPizza = true;
        }
    }

    //randomize aray sistemi
    private T GetRandomElement<T>(T[] array)
    {
        if (array != null && array.Length > 0)
        {
            int randomIndex = Random.Range(0, array.Length);
            return array[randomIndex];
        }
        else
        {
            return default(T);
        }
    }

    // Verilen değerin dizideki indeksini bulan fonksiyon
    private int GetIndexInArray<T>(T value, T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(value, array[i]))
            {
                return i;
            }
        }

        return -1; // Eğer bulunamazsa -1 döndür
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
}