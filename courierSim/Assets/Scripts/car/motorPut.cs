using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motorPut : MonoBehaviour
{
    public GameObject pizzaOrderObj;
    public GameObject burgerOrderObj;

    public bool isPizza;
    public bool isBurger;

    void Start()
    {
        pizzaOrderObj.SetActive(false);
        burgerOrderObj.SetActive(false);
        isPizza = false;
        isBurger = false;
    }

    void Update()
    {
        interact();
    }

    public void interact()
    {
        if (isPizza)
        {
            pizzaOrderObj.SetActive(true);
        }

        if (isBurger)
        {
            burgerOrderObj.SetActive(true);
        }
        // else if(!isPizza && !isBurger)
        // {
        //     pizzaOrderObj.SetActive(false);
        //     burgerOrderObj.SetActive(false);
        // }
        
    }
}