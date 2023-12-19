using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSpawner : MonoBehaviour
{
    public Texture pp;
    public scribtableOrders ScribtableOrders;
    public Transform parentTransform;

    void Start()
    {
    }

    void Update()
    {
        spawnTransform();
    }

    private void spawnTransform()
    {
        if (OrderManager.instance.isSpawn)
        {
            GameObject newOrder = Instantiate(ScribtableOrders.OrderPrefab, parentTransform.position,
                Quaternion.identity, parentTransform);

            OrderManager.instance.isSpawn = false;
        }
    }
}