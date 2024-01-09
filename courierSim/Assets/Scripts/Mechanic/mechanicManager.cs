using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class mechanicManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject mechanicUI;
    public GameObject EnterMechanicPosRes;
    public TMP_Text DamageText;

    public TMP_Text DamagePayText;
    public int DamagePay;
    private int damageValue;

    void Start()
    {
        mechanicUI.gameObject.SetActive(false);
    }

    void Update()
    {
        Mechanic();
        if (interact.instance.isMotor)
        {
            EnterRepairPos();
        }
        //text yazdırma
        DamagePayText.text = DamagePay.ToString();
        DamageText.text = "%" + damageValue.ToString();
    }

    private void Mechanic()
    {
        if (interact.instance.isMechanic)
        {
            mechanicUI.gameObject.SetActive(true);
        }
    }

    public void Reapir()
    {
        if (phoneMenu.instance.price > DamagePay)
        {
            carMovement.instance.carDamageValue = 0;
            phoneMenu.instance.price -= DamagePay;
            DamagePay = 0;
        }
    }

    public void Back()
    {
        interact.instance.isMechanic = false;
        mechanicUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("motor1"))
        {
            float damageValueF = other.GetComponent<carMovement>().carDamageValue;
            int damageValue = Mathf.RoundToInt(damageValueF);

            DamagePay += damageValue;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("motor1"))
        {
            float damageValueF = other.GetComponent<carMovement>().carDamageValue;
            damageValue = Mathf.RoundToInt(damageValueF);
        }
    }

    private void EnterRepairPos()
    {
        if (carMovement.instance.isVeryDamage && !interact.instance.isMechanic)
        {
            EnterMechanicPosRes.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Player.transform.localPosition = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
            }
        }
        else
        {
            EnterMechanicPosRes.SetActive(false);
        }
    }
}