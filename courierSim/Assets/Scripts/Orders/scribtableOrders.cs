using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "OrderMenu")]
public class scribtableOrders :ScriptableObject
{
    public string[] CustomerNames;
    public GameObject BurgerPositionObj;
    public GameObject PizzaPositionObj;
    public GameObject DeliveryPositionObj;

}
