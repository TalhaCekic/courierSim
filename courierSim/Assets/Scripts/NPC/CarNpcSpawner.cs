using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CarNpcSpawner : MonoBehaviour
{
    public scribtableNpcWay scribTableNpcWay;
    void Start()
    {
        Instantiate(scribTableNpcWay.car, this.transform.position, this.transform.rotation);
    }

    void Update()
    {
        
    }
}
