using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public Transform target;        
    public float moveSpeed = 2f;    
    private EnemyPathfinding pathfinding; 
    private List<GridNode> path;         
    private int currentPathIndex;    

    void Start()
    {
        pathfinding = GetComponent<EnemyPathfinding>();
        path = new List<GridNode>();
        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        FollowPath();
    }

    IEnumerator UpdatePath()
    {
        while (true)
        {
            if (target != null)
            {
                Vector2Int start = WorldPositionToGrid(transform.position);
                Vector2Int targetPos = WorldPositionToGrid(target.position);
                path = pathfinding.FindPath(start, targetPos);
                currentPathIndex = 0;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void FollowPath()
    {
        if (path == null || path.Count == 0) return;

        if (currentPathIndex < path.Count)
        {
            Vector3 targetPosition = path[currentPathIndex].worldPosition;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentPathIndex++;
            }
        }
    }

    Vector2Int WorldPositionToGrid(Vector3 worldPosition)
    {
        return pathfinding.gridSystem.GetNodeFromWorldPosition(worldPosition).gridPosition;
    }

    Vector3 GridPositionToWorld(Vector2Int gridPosition)
    {
        return pathfinding.gridSystem.GetWorldPositionFromNode(
            pathfinding.gridSystem.GetNodeFromWorldPosition(new Vector3(gridPosition.x, gridPosition.y, 0)));
    }
}
