using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;

    public scribtableOrders scribtableOrders;
    public string Order;
    public string orderName;
    public GameObject[] BurgerPositions;
    public GameObject[] PizzaPositions;
    public GameObject[] DeliveryPositions;

    public GameObject selectedBurgerPosition;
    public GameObject selectedPizzaPosition;
    public GameObject selectedDeliveryPosition;

    private float minInterval = 10f; //10
    private float maxInterval = 15f; //20
    public float nextOrderTime = 0f;

    public bool isSpawn;
    public bool isOrder;


    void Start()
    {
        instance = this;
        isSpawn = false;
        BurgerPositions = GameObject.FindGameObjectsWithTag("BurgerShop");

        PizzaPositions = GameObject.FindGameObjectsWithTag("PizzaShop");
        DeliveryPositions = GameObject.FindGameObjectsWithTag("DeliveryPosition");
        selectedBurgerPosition = BurgerPositions[0];
        selectedPizzaPosition = PizzaPositions[0];
    }

    void Update()
    {
        if (Time.time >= nextOrderTime)
        {
            timeOrderSpawn();
            spawnOrderPosition();
        }
    }

    public void timeOrderSpawn()
    {
        if (!isOrder)
        {
            nextOrderTime = Time.time + Random.Range(minInterval, maxInterval);
            isOrder = true;
        }
       
    }

    public void spawnOrderPosition()
    {
        Order = GetRandomElement(scribtableOrders.order);
        orderName = GetRandomElement(scribtableOrders.CustomerNames);
        selectedDeliveryPosition = GetRandomElement(DeliveryPositions);
        print(orderName);
        print(Order);
        print(selectedDeliveryPosition);
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
}