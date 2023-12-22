using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaShop : MonoBehaviour
{
    public GameObject selected;
    void Start()
    {
        selected.SetActive(false);
    }

    void Update()
    {
        if (OrderManager.instance.isPizza)
        {
            if (!interact.instance.isHasPizza)
            {
                selected.SetActive(true);
            }
            else
            {
                selected.SetActive(false);
            }
        }
    }
}
