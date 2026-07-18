using System;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 6;
    public int height = 4;
    public GameObject gridCellPrefab;
    public List<GameObject> gridObjects = new List<GameObject>();
    public GameObject[,] gridCells;

    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        gridCells = new GameObject[width, height];
        Vector2 centerOffset = new Vector2(width-1f, 0);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 gridPosition = new Vector2(x*2, y*2);
                Vector2 spawnPosition = gridPosition - centerOffset;

                GameObject gridCell = Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity);

                gridCell.transform.SetParent(transform);

                gridCell.GetComponent<GridCell>().gridIndex = gridPosition/2;

                gridCells[x, y] = gridCell;
            }
        }
    }

    public bool AddObjectToGrid(GameObject obj, Vector2 gridPosition)
    {
        if (gridPosition.x > width || gridPosition.x < 0 || gridPosition.y > height || gridPosition.y < 0)
            return false;
        GridCell cell = gridCells[(int)gridPosition.x, (int)gridPosition.y].GetComponent<GridCell>();
        if (cell.cellTaken) 
            return false;
        GameObject newObj = Instantiate(obj, cell.GetComponent<Transform>().position, Quaternion.identity);
        newObj.transform.SetParent(transform);
        gridObjects.Add(newObj);
        cell.objectIncell = newObj;
        cell.cellTaken = true;
        return true;
    }
}
