using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using TMPro;

public class GridSystem
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,] gridArray;
    private TextMeshPro[,] debugTextArray;


    public GridSystem(int width, int height, float cellSize, Vector3 originPosition, Transform debugPrefab)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[width, height];
        debugTextArray = new TextMeshPro[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Transform transform = Object.Instantiate(debugPrefab,
                    GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * 0.5f, Quaternion.identity);
                debugTextArray[x, z] = transform.GetComponentInChildren<TextMeshPro>();
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public void SetValue(int x, int z, int value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridArray[x, z] = value;
            debugTextArray[x, z].text = value.ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    public int GetValue(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int z);
        return GetValue(x, z);
    }
}