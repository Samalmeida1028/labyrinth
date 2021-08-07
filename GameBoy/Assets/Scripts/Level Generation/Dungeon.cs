using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{
    public float width;
    public float height;
    public Vector2 botLeft;
    public Vector2 topRight;

    public int depth;

    public Dungeon parentDungeon;
    public Dungeon leftDungeon;
    public Dungeon rightDungeon;

    public Split split = null;

    public bool failSplit = false;

    public Room room = null;

    public Dungeon (Vector2 botLeft, Vector2 topRight, int depth)
    {
        this.botLeft = botLeft;
        this.topRight = topRight;

        width = topRight.x - botLeft.x;
        height = topRight.y-botLeft.y;

        this.split = null;
        this.parentDungeon = null;
        
        this.depth = depth;
    }

    public void setRightDungeon(Dungeon d)
    {
        rightDungeon = (d);
    }

    public void setLeftDungeon(Dungeon d)
    {
        leftDungeon = (d);
    }

    public Split setSplit(Split s)
    {
        split = s;
        return s;
    }

    public void setParent(Dungeon d)
    {
        this.parentDungeon = d;
    }

    public void setRoom(Room r){
        this.room = r;
    }

    public Room getRoom()
    {
        
        if(room!=null)
        {
            return room;
        }
        Room leftRoom=null;
        Room rightRoom=null;

        if(leftDungeon!=null)
        {
            leftRoom = leftDungeon.getRoom();
        }

        if(rightDungeon!=null)
        {
            rightRoom = rightDungeon.getRoom();
        }

        if(leftRoom == null && rightRoom == null)
        {
            return null;
        }
        else if(leftRoom==null)
        {
            return rightRoom;
        }
        else if(rightRoom==null)
        {
            return leftRoom;
        }
        else if(Random.Range(0,2)==1)
        {
            return leftRoom;
        }
        else
        {
            return rightRoom;
        }
    }
}
