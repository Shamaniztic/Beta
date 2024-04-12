using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public Tilemap tilemap;
    public float nodeSize = 1f;

    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return tilemap.CellToWorld(new Vector3Int(gridPosition.x, gridPosition.y, 0)) + new Vector3(nodeSize / 2f, nodeSize / 2f, 0f);
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        Vector3Int gridPosition = tilemap.WorldToCell(worldPosition);
        return new Vector2Int(gridPosition.x, gridPosition.y);
    }

    public bool IsWithinGrid(Vector2Int gridPosition)
    {
        return tilemap.HasTile(new Vector3Int(gridPosition.x, gridPosition.y, 0));
    }

    public bool IsWalkable(Vector2Int gridPosition)
    {
        // Add your logic to determine if the grid position is walkable
        // Return true if walkable, false otherwise
        return true;
    }
}
