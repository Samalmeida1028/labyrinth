using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObj : MonoBehaviour
{
    public int roomNumber;
    public Rect roomDimensions;
    public Vector2 botLeft;
    public Vector2 topRight;
    public Vector2 roomCenter;
    public GameObject [,] roomGrid;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void setRoom(Room r){
        this.roomDimensions = r.roomDimensions;
        this.roomNumber = r.roomNumber;
        this.roomCenter = r.roomCenter;
        this.botLeft = r.botLeft;
        this.topRight = r.topRight;
    }
    
    public void setGrid(GameObject [,] roomGrid)
    {

        this.roomGrid = new GameObject[roomGrid.GetLength(0),roomGrid.GetLength(1)];

        for (int i = 0; i < roomGrid.GetLength(1); i++)
        {
            for (int j = 0; j < roomGrid.GetLength(0); j++)
            {
                this.roomGrid[j,i] = roomGrid[j,i];
            }
        }

    }
}
