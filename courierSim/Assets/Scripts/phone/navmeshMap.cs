using UnityEngine.AI;
using UnityEngine;

public class navmeshMap : MonoBehaviour
{
    public Transform target;
    public LineRenderer lineRenderer;

    private NavMeshPath navMeshPath;
    private NavMeshAgent navMeshAgent;
    
    public const int CustomAreaIndex = 1;

    void Start()
    {
        navMeshPath = new NavMeshPath();
        navMeshAgent = GetComponent<NavMeshAgent>();
        DrawPath();
    }

    private void SelectTarget()
    {
        if (OrderManager.instance.isBurger && !interact.instance.isBurgerYes)
        {
            target = BurgerShop.instance.Target;
        } 
        if (OrderManager.instance.isPizza && !interact.instance.isPizzaYes)
        {
            target = PizzaShop.instance.Target;
        }
    }
    void DrawPath()
    {
        SelectTarget();
        if (target == null)
            return;
        
        int customAreaMask = 1 << CustomAreaIndex;
        int navMeshAreaMask = NavMesh.AllAreas & ~customAreaMask;
        
        navMeshAgent.SetDestination(target.position);
        NavMesh.CalculatePath(transform.position, target.position, navMeshAreaMask, navMeshPath);

        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            Debug.LogWarning("Path not complete");
            return;
        }
        lineRenderer.positionCount = 0;

        for (int i = 0; i < navMeshPath.corners.Length; i++)
        {
            print("girse");
            AddPointToLineRenderer(navMeshPath.corners[i]);
        }
    }

    void AddPointToLineRenderer(Vector3 point)
    {
        // Line Renderer'a nokta ekleyin
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
    }

    void Update()
    {
        DrawPath();
    }
}
