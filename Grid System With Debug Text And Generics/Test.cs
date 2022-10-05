using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Test : MonoBehaviour
{
    public GridSystem<GridTestClass> gridSystem;
    [SerializeField] private Transform gridDebugObjectPrefab;
    private void Start()
    {
        gridSystem = new GridSystem<GridTestClass>(64 ,64,2,new Vector3(-64,0,-64), (GridSystem<GridTestClass> g,int x, int z) => new GridTestClass(g,x,z), gridDebugObjectPrefab);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gridSystem.GetGridObject(GetPosition3D()).AddValue(12);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(gridSystem.GetGridObject(GetPosition3D()));
        }
    }

    public Vector3 GetPosition3D()
    {
        Ray ray = Instance._cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, Instance.mousePlaneLayerMask);
        return hit.point;
    }

    public class GridTestClass
    {

        private GridSystem<GridTestClass> grid;
        private int x, z;
        
        public int value;

        public GridTestClass(GridSystem<GridTestClass> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }
        public void AddValue(int addValue)
        {
            value += addValue;
            grid.TriggerGridObjectChanged(x,z);
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
    
    
    
    
}