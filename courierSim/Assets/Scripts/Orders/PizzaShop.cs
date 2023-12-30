using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaShop : MonoBehaviour
{
    public static PizzaShop instance;
    public GameObject selected;
    public bool isSelect;
    void Start()
    {
        instance = this;
        selected.SetActive(false);
    }

    void Update()
    {
        if (OrderManager.instance.isPizza)
        {
            if (!interact.instance.isPizzaYes)
            {
                selected.SetActive(true);
                isSelect = false;
            }
            else
            {
                selected.SetActive(false);
                isSelect = true;
            }
        }
    }
}
