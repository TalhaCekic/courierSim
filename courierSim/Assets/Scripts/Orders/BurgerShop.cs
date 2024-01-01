using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerShop : MonoBehaviour
{
    public static BurgerShop instance;
    public Transform Target;
    public GameObject selected;
    public bool isSelect;
    void Start()
    {
        instance = this;
        selected.SetActive(false);
    }

    void Update()
    {
        if (OrderManager.instance.isBurger )
        {
            if (!interact.instance.isBurgerYes)
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
