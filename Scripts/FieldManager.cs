using UnityEngine;
using System.Collections.Generic;

public class FieldManager : MonoBehaviour
{
    public static FieldManager Instance;

    [Header("Grid Settings")]
    public int rows = 3;
    public int cols = 3;
    public float cellSize = 2f;   // spacing between cells
    public Vector3 fieldOrigin = Vector3.zero; // top-left or center position

    private bool[,] occupied;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        occupied = new bool[rows, cols];
    }

    public Vector3? GetFreeCell()
    {
        List<Vector2Int> freeCells = new List<Vector2Int>();

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (!occupied[r, c])
                    freeCells.Add(new Vector2Int(r, c));
            }
        }

        if (freeCells.Count == 0) return null; // no space

        Vector2Int chosen = freeCells[Random.Range(0, freeCells.Count)];
        occupied[chosen.x, chosen.y] = true;
        return CellToWorld(chosen.x, chosen.y);
    }

    public void FreeCell(Vector3 worldPos)
    {
        // Find nearest cell to free
        Vector2Int cell = WorldToCell(worldPos);
        if (cell.x >= 0 && cell.x < rows && cell.y >= 0 && cell.y < cols)
            occupied[cell.x, cell.y] = false;
    }

    private Vector3 CellToWorld(int row, int col)
    {
        return fieldOrigin + new Vector3(col * cellSize, 0, -row * cellSize);
    }

    private Vector2Int WorldToCell(Vector3 pos)
    {
        int col = Mathf.RoundToInt((pos.x - fieldOrigin.x) / cellSize);
        int row = Mathf.RoundToInt(-(pos.z - fieldOrigin.z) / cellSize);
        return new Vector2Int(row, col);
    }
}
