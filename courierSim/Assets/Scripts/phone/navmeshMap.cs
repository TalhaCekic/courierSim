using UnityEngine.AI;
using UnityEngine;

public class navmeshMap : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent navMeshAgent;
    private LineRenderer lineRenderer;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (OrderManager.instance.isBurger)
        {
            if (interact.instance.isHasBurger)
            {
                target = OrderManager.instance.selectedDeliveryPosition.transform; 
            }
            else
            {
                target = OrderManager.instance.BurgerPositions[0].transform; 
            }
           
        }
        if (OrderManager.instance.isPizza)
        {
            if (interact.instance.isHasPizza)
            {
                target = OrderManager.instance.selectedDeliveryPosition.transform; 
            }
            else
            {
                target = OrderManager.instance.PizzaPositions[0].transform; 
            }
        }
        if (target != null && navMeshAgent.remainingDistance < 0.1f)
        {
            SetDestination();
        }
        // Yolu çiz
        DrawPath();
    }

    void SetDestination()
    {
        navMeshAgent.SetDestination(target.position);
    }

    void DrawPath()
    {
        // Yol var mı kontrolü
        if (navMeshAgent.path.corners.Length < 2)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        // LineRenderer'ı başlat
        lineRenderer.positionCount = navMeshAgent.path.corners.Length;
        lineRenderer.SetPositions(navMeshAgent.path.corners);
        lineRenderer.enabled = true;
    }
}
