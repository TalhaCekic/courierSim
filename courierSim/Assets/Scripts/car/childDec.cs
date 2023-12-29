using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class childDec : MonoBehaviour
{
    //public Transform childOrder;
    void Start()
    {
        
    }

    void Update()
    {
        this.transform.position = new Vector3(0, 0, 0);
        print(this.transform.position);
        this.transform.rotation =  Quaternion.Euler(0, 0, 0);
        // childOrder = this.transform.GetChild(0);
        //          print("alsana");
        // if (childOrder != null)
        // {
        //     childOrder.transform.position = Vector3.up;
        //     childOrder.transform.rotation =  Quaternion.Euler(0, 0, 0);
        //     childOrder.transform.localScale =  new Vector3(0,0,0);
        // }

    }
}
