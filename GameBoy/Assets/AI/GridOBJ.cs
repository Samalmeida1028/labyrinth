using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CodeMonkey.Utils;

public class GridOBJ<TGridObject>
{
    public event EventHandler<OnGridOBJValueChangedEventArgs> OnGridOBJValueChanged;
    public class OnGridOBJValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    private int width;
    private int height;
    private TGridObject[,] gridArray;


    private float cellSize;
    private Vector3 origin = new Vector3(0f, 0f, 0f);


    public GridOBJ(int width, int height, float cellSize, Vector3 origin, Func<GridOBJ<TGridObject>, int, int ,TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }

        }

        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugGridArray = new TextMesh[width, height];


            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugGridArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPos(x, y) + (new Vector3(cellSize, cellSize) * .5f), 5, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), Color.white, 30f);
                    Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.white, 30f);


                }

            }
            Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), Color.white, 30f);
            Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.white, 30f);


            OnGridOBJValueChanged += (object sender, OnGridOBJValueChangedEventArgs eventArgs) =>
            {
                debugGridArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }

    }


    private Vector3 GetWorldPos(int x, int y)
    {
        return ((new Vector3(x, y) * cellSize) + origin);
    }


    private void GetXY(Vector3 worldPos, out int x, out int y)
    {

        x = Mathf.FloorToInt((worldPos - origin).x / cellSize);
        y = Mathf.FloorToInt((worldPos - origin).y / cellSize);


    }


    public void TriggerGridOBJChanged(int x, int y){
        if(OnGridOBJValueChanged != null) OnGridOBJValueChanged(this,new OnGridOBJValueChangedEventArgs{x=x,y=y});
    }

    public void SetGridOBJ(int x, int y, TGridObject val)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = val;
            if(OnGridOBJValueChanged != null) OnGridOBJValueChanged(this,new OnGridOBJValueChangedEventArgs{x=x,y=y});

        }

    }
    public void SetGridOBJ(Vector3 pos, TGridObject val)
    {
        int x, y;
        GetXY(pos, out x, out y);
        SetGridOBJ(x, y, val);
    }



    public TGridObject GetGridOBJ(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else return default(TGridObject);


    }

    public TGridObject GetGridOBJ(Vector3 pos)
    {
        int x, y;
        GetXY(pos, out x, out y);
        return GetGridOBJ(x, y);


    }

}
