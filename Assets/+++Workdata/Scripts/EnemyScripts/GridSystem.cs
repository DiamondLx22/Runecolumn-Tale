using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
       public Vector2Int gridSize; 
    public float cellSize;      
    public LayerMask unwalkableMask; 

    private GridNode[,] grid;   

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GridNode[gridSize.x, gridSize.y];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.up * gridSize.y / 2;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * cellSize + cellSize / 2) + Vector3.up * (y * cellSize + cellSize / 2);
                bool walkable = !Physics2D.OverlapCircle(worldPoint, cellSize / 2, unwalkableMask);
                grid[x, y] = new GridNode(new Vector2Int(x, y), worldPoint, walkable);
            }
        }
    }

    public GridNode GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x + gridSize.x * cellSize / 2) / cellSize);
        int y = Mathf.RoundToInt((worldPosition.y + gridSize.y * cellSize / 2) / cellSize);
        x = Mathf.Clamp(x, 0, gridSize.x - 1);
        y = Mathf.Clamp(y, 0, gridSize.y - 1);
        return grid[x, y];
    }

    public Vector3 GetWorldPositionFromNode(GridNode node)
    {
        return node.worldPosition;
    }

    
    void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (GridNode node in grid)
            {
                Gizmos.color = (node.isWalkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (cellSize - 0.1f));
            }
        }
    }
}

/*public class GridNode
{
    public Vector2Int gridPosition;  
    public Vector3 worldPosition;    
    public bool isWalkable;          

    public GridNode(Vector2Int gridPos, Vector3 worldPos, bool walkable)
    {
        gridPosition = gridPos;
        worldPosition = worldPos;
        isWalkable = walkable;
    }
}*/
