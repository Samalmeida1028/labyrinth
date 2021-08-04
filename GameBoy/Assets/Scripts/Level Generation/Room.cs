using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int roomNumber;
    public Rect roomDimensions;
    public Vector2 botLeft;
    public Vector2 topRight;
    public Vector2 roomCenter;
    

    public Room (int roomNumber, float width, float height, Vector2 botLeft, Vector2 topRight, Vector2 roomCenter){
        
        this.roomNumber = roomNumber;
        this.botLeft = botLeft;
        this.topRight = topRight;
        this.roomCenter = roomCenter;

        this.roomDimensions = new Rect(botLeft.x,botLeft.y,width,height);

    }
    

}
