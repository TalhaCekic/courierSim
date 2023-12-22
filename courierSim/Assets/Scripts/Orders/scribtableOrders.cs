using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "OrderMenu")]
public class scribtableOrders :ScriptableObject
{
    public string[] CustomerNames;
    public string[] order;
    public GameObject BurgerPositionObj;
    public GameObject PizzaPositionObj;
    public GameObject DeliveryPositionObj;
    public int[] orderPrice;
    public int[] tipPrice;
    public int[] orderTimes;

    public GameObject OrderPrefab;
    public GameObject deliverySelectedIU;

}
