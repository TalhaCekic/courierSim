using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;
    
    private scribtableOrders scribtableOrders;
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
    

    void Start()
    {
        instance = this;
        isSpawn = false;
        BurgerPositions = GameObject.FindGameObjectsWithTag("BurgerShop");
        PizzaPositions = GameObject.FindGameObjectsWithTag("PizzaShop");
        DeliveryPositions = GameObject.FindGameObjectsWithTag("DeliveryPosition");
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
        nextOrderTime = Time.time + Random.Range(minInterval, maxInterval);
    }
    
    public void spawnOrderPosition()
    {
        if (isSpawn)
        {
            selectedDeliveryPosition = GetRandomElement(DeliveryPositions);
            print(selectedDeliveryPosition);
            isSpawn = false;
        }
    }
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