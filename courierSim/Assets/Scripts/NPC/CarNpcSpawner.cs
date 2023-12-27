using UnityEngine;

public class CarNpcSpawner : MonoBehaviour
{
    public static CarNpcSpawner instance;
    public scribtableNpcWay scribTableNpcWay;
    //public GameObject[] ways1;
    public bool isStartSpawn;
    public float selecetWayDelay;

    void Start()
    {
        instance = this;
    }
    
    void Update()
    {
        if (isStartSpawn)
        {
            if (selecetWayDelay < 0f)
            {
                Instantiate(scribTableNpcWay.car, this.transform.position, this.transform.rotation);
                
                selecetWayDelay = 5;
                isStartSpawn = false;
            }
            else
            {
                selecetWayDelay -= Time.deltaTime;
            }
        }
    }
}