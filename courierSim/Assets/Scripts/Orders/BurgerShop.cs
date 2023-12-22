using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerShop : MonoBehaviour
{
    public GameObject selected;
    void Start()
    {
        selected.SetActive(false);
    }

    void Update()
    {
        if (OrderManager.instance.isBurger )
        {
            if (!interact.instance.isHasBurger)
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
