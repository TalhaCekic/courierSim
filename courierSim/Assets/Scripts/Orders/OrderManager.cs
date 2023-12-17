using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    private scribtableOrders scribtableOrders;
    public GameObject[] BurgerPositions;
    public GameObject[] PizzaPositions;
    public GameObject[] DeliveryPositions;
    void Start()
    {
        BurgerPositions = GameObject.FindGameObjectsWithTag("BurgerShop");
        PizzaPositions = GameObject.FindGameObjectsWithTag("PizzaShop");
        DeliveryPositions = GameObject.FindGameObjectsWithTag("DeliveryPosition");
    }

    void Update()
    {
        
    }
}
