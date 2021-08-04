using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridOBJ
{

    private int width;
    private int height;
    private int[,] gridArray;
    private TextMesh[,] debugGridArray;

    private float cellSize;
    private Vector3 origin = new Vector3(0f, 0f, 0f);


    public GridOBJ(int width, int height, float cellSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new int[width, height];
        debugGridArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                debugGridArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPos(x, y) + (new Vector3(cellSize, cellSize) * .5f), 5, Color.white, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x, y + 1), Color.white, 30f);
                Debug.DrawLine(GetWorldPos(x, y), GetWorldPos(x + 1, y), Color.white, 30f);


            }

        }
        Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height), Color.white, 30f);
        Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height), Color.white, 30f);

    }


    private Vector3 GetWorldPos(int x, int y)
    {
        return ((new Vector3(x, y) * cellSize) + origin);
    }

    public void SetVal(int x, int y, int val)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] += val;
            debugGridArray[x, y].text = gridArray[x, y].ToString();


        }

    }

    private void GetXY(Vector3 worldPos, out int x, out int y)
    {

        x = Mathf.FloorToInt((worldPos-origin).x / cellSize);
        y = Mathf.FloorToInt((worldPos-origin).y / cellSize);


    }


    public void SetVal(Vector3 pos, int val)
    {
        int x, y;
        GetXY(pos, out x, out y);
        SetVal(x, y, val);
    }

    public int GetVal(int x, int y){
        if (x >= 0 && y >= 0 && x < width && y < height){
            return gridArray[x,y];
        }
        else return 0;


    }

        public int GetVal(Vector3 pos){
            int x, y;
            GetXY(pos, out x, out y);
            return GetVal(x,y);


    }



}
