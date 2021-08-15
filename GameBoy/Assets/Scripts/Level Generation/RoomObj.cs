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
    public bool isSpawnRoom = false;
    public bool isEndRoom = false;
    public bool isShop = false;
    public int chestCount = 0;
    public int enemyCount = 0;

    public void setRoom(Room r)
    {
        this.roomDimensions = r.roomDimensions;
        this.roomNumber = r.roomNumber;
        this.roomCenter = r.roomCenter;
        this.botLeft = r.botLeft;
        this.topRight = r.topRight;
    }

    public void setChestCount(int c)
    {
        this.chestCount = c;
    }
}
