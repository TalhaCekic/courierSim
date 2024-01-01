using UnityEngine.AI;
using UnityEngine;

public class navmeshMap : MonoBehaviour
{
    public Transform target;
    public LineRenderer lineRenderer;

    private NavMeshPath navMeshPath;

    public const int CustomAreaIndex = 1;

    void Start()
    {
        navMeshPath = new NavMeshPath();
        target = null;
        DrawPath();
    }

    void Update()
    {
        DrawPath();
    }

    private void SelectTarget()
    {
        if (OrderManager.instance.isOrder)
        {
            lineRenderer.enabled = true;
            if (OrderManager.instance.isBurger && !interact.instance.isBurgerYes)
            {
                target = BurgerShop.instance.Target;
            }
            else if (OrderManager.instance.isBurger && interact.instance.isBurgerYes)
            {
                target = OrderManager.instance.selectedDeliveryPosition.transform;
            }

            if (OrderManager.instance.isPizza && !interact.instance.isPizzaYes)
            {
                target = PizzaShop.instance.Target;
            }
            else if (OrderManager.instance.isPizza && interact.instance.isPizzaYes)
            {
                target = OrderManager.instance.selectedDeliveryPosition.transform;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void DrawPath()
    {
        SelectTarget();
        if (target != null)
        {
            int customAreaMask = 1 << CustomAreaIndex;
            int navMeshAreaMask = NavMesh.AllAreas & ~customAreaMask;

            NavMesh.CalculatePath(transform.position, target.position, navMeshAreaMask, navMeshPath);
            lineRenderer.positionCount = 0;
            
            for (int i = 0; i < navMeshPath.corners.Length; i++)
            {
                AddPointToLineRenderer(navMeshPath.corners[i]);
            }
        }
    }

    void AddPointToLineRenderer(Vector3 point)
    {
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
    }
}