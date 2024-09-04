using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public Vector2Int gridPosition;
    public Vector3 worldPosition; 
    public bool isWalkable;
    public GridNode parent;

    public GridNode(Vector2Int gridPosition, Vector3 worldPos, bool isWalkable)
    {
        this.gridPosition = gridPosition;
        this.isWalkable = isWalkable;
        worldPosition = worldPos;
    }
}
