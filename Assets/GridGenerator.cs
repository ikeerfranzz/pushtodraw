using UnityEngine;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    public static GridGenerator gridGenerator;
    public int gridSize = 8;
    [SerializeField] List<List<GameObject>> gridObjects;
    public GameObject gridCellTemplate;

    [ContextMenu("Generate Grid")]
    void GenerateGrid()
    {
        DeleteGrid();
        gridObjects = new List<List<GameObject>>();
        for (int i = 0; i < gridSize; i++)
        {
            List<GameObject> list = new List<GameObject>();
            for (int j = 0; j < gridSize; j++)
            {
                Vector2 cellPos = (Vector2)transform.position + Vector2.left * gridSize / 2 + Vector2.up * gridSize / 2;
                cellPos = cellPos + Vector2.right * j + Vector2.down * i;
                GameObject gridCell = Instantiate(gridCellTemplate, cellPos, Quaternion.identity);
                list.Add(gridCell);
            }
            gridObjects.Add(list);
        }
    }

    [ContextMenu("Delete Grid")]
    void DeleteGrid()
    {
        if (gridObjects != null)
        {
            int size = gridObjects.Count;
            for (int i = 0; i < size; i++)
            {
                int size2 = gridObjects[i].Count;
                for (int j = 0; j < size2; j++)
                {
                    DestroyImmediate(gridObjects[i][j]);
                }
                gridObjects[i].Clear();
            }
            gridObjects.Clear();
        }
    }
}
