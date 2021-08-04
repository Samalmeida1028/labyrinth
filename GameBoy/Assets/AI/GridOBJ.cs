using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridOBJ{

    private int width;
    private int height;
    private int[,] gridArray;
    private TextMesh[,] debugGridArray;

    private float cellSize;
    private Vector3 origin=new Vector3 (0f,0f,0f);


    public GridOBJ(int width, int height, float cellSize, Vector3 origin){
        this.width= width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        Debug.Log(origin);

        gridArray = new int[width,height];
        debugGridArray = new TextMesh[width,height];

        for(int x = 0; x < gridArray.GetLength(0); x++){
            for (int y = 0; y < gridArray.GetLength(1); y++){
                debugGridArray[x,y] = UtilsClass.CreateWorldText(gridArray[x,y].ToString(),null,GetWorldPos(x,y)+ (new Vector3(cellSize,cellSize) *.5f),5,Color.white,TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPos(x,y),GetWorldPos(x,y+1),Color.white,30f);
                Debug.DrawLine(GetWorldPos(x,y),GetWorldPos(x+1,y),Color.white,30f);


            }

        }
        Debug.DrawLine(GetWorldPos(0,height),GetWorldPos(width,height),Color.white,30f);
        Debug.DrawLine(GetWorldPos(width,0),GetWorldPos(width,height),Color.white,30f);

    } 


    private Vector3 GetWorldPos(int x, int y){
        return ((new Vector3(x,y) * cellSize)+origin);
    }

    public void SetVal(int x, int y, int val){
        if(x>=0&&y>=0&&x<=width&&y<=height){
            gridArray[x,y] = val;
            Debug.Log(x + "  " + y);
            Debug.Log("grid under here");
            Debug.Log("grid val = " +gridArray[x,y]);


        }

    }

    private void GetXY(Vector3 worldPos, out int x, out int y){

        x = Mathf.FloorToInt((worldPos.x/ cellSize)-(origin.x/cellSize));
        y = Mathf.FloorToInt((worldPos.y/ cellSize)- (origin.y/cellSize));
        Debug.Log("get 2");


    }


    public void SetVal(Vector3 pos, int val){
        int x,y;
        Debug.Log("get 1");
        GetXY(pos,out x,out y);
        Debug.Log("set 2");
        SetVal(x,y,val);
    }

    

}
