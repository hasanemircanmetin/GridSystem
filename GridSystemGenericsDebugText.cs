using System;
using UnityEngine;
using TMPro;

public class GridSystem<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    private TextMeshPro[,] debugTextArray;
    
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int z;
    }

    public GridSystem(int width, int height, float cellSize, Vector3 originPosition,
        Func<GridSystem<TGridObject>, int, int, TGridObject> createGridObject, Transform debugPrefab)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMeshPro[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                gridArray[x, z] = createGridObject(this,x,z);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Transform transform = GameObject.Instantiate(debugPrefab,
                    GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * 0.5f, Quaternion.identity);
                debugTextArray[x, z] = transform.GetComponentInChildren<TextMeshPro>();
                debugTextArray[x, z].text = gridArray[x, z]?.ToString();
                OnGridObjectChanged += (_, eventArgs) => {
                    debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
                };
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

    public void SetGridObject(int x, int z, TGridObject value)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridArray[x, z] = value;
            debugTextArray[x, z].text = value.ToString();
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value)
    {
        GetXY(worldPosition, out int x, out int z);
        SetGridObject(x, z, value);
        OnGridObjectChanged?.Invoke(this,new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    public TGridObject GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return gridArray[x, z];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        OnGridObjectChanged?.Invoke(this,new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int z);
        return GetGridObject(x, z);
    }
}