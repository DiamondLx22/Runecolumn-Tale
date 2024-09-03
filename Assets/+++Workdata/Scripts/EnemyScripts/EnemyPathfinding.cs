using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
  
    public Transform target;
    public Transform enemy;
    public Vector2Int gridSize;
    public float cellSize;
    public LayerMask unwalkableMask;

    public GridSystem gridSystem;

    void Start()
    {
        gridSystem = FindObjectOfType<GridSystem>();
    }
    

    public List<GridNode> FindPath(Vector2Int startPos, Vector2Int targetPos)
    {
        GridNode startNode = gridSystem.GetNodeFromWorldPosition(GridPositionToWorld(startPos));
        GridNode targetNode = gridSystem.GetNodeFromWorldPosition(GridPositionToWorld(targetPos));

        Queue<GridNode> queue = new Queue<GridNode>();
        HashSet<GridNode> visited = new HashSet<GridNode>();
        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            GridNode currentNode = queue.Dequeue();

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (GridNode neighbor in GetNeighbors(currentNode))
            {
                if (!neighbor.isWalkable || visited.Contains(neighbor))
                    continue;

                neighbor.parent = currentNode;
                queue.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }

        return null;
    }

    List<GridNode> GetNeighbors(GridNode node)
    {
        List<GridNode> neighbors = new List<GridNode>();

        Vector2Int[] directions = {
            new Vector2Int(0, 1),  
            new Vector2Int(1, 0),  
            new Vector2Int(0, -1), 
            new Vector2Int(-1, 0)  
        };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = node.gridPosition + direction;

            if (neighborPos.x >= 0 && neighborPos.x < gridSize.x && neighborPos.y >= 0 && neighborPos.y < gridSystem.gridSize.y)
            {
                neighbors.Add(gridSystem.GetNodeFromWorldPosition(GridPositionToWorld(neighborPos)));
            }
        }

        return neighbors;
    }

    List<GridNode> RetracePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode> path = new List<GridNode>();
        GridNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
    
    Vector3 GridPositionToWorld(Vector2Int gridPosition)
    {
        return gridSystem.GetWorldPositionFromNode(gridSystem.GetNodeFromWorldPosition(new Vector3(gridPosition.x, gridPosition.y, 0)));
    }
}
