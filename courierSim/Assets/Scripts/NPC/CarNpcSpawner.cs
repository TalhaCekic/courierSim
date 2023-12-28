using UnityEngine;

public class CarNpcSpawner : MonoBehaviour
{
    public int selectWayDecSpawner;

    public static CarNpcSpawner instance;

    public scribtableNpcWay scribTableNpcWay;

    public bool isStartSpawn;
    public float selecetWayDelay;

    public Transform detector;
    public LayerMask CarMask;

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

        WaySpawnerDetector();
    }

    public void WaySpawnerDetector()
    {
        Ray decRay = new Ray(detector.position, detector.forward);
        Debug.DrawLine(decRay.origin, decRay.origin + decRay.direction * 3, Color.white);

        RaycastHit hit;
        if (Physics.Raycast(decRay, out hit, 3, CarMask))
        {
            NpcCarMovement npcCarMovement = hit.collider.gameObject.GetComponent<NpcCarMovement>();
            npcCarMovement.selectWayDec = selectWayDecSpawner;
        }
    }
}