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
    public List <Dungeon> dungeonList;

    public Split split = null;

    public bool failSplit = false;

    public Dungeon (Vector2 botLeft, Vector2 topRight, int depth)
    {
        this.botLeft = botLeft;
        this.topRight = topRight;

        width = topRight.x - botLeft.x;
        height = topRight.y-botLeft.y;

        this.split = null;
        this.parentDungeon = null;
        this.dungeonList = new List <Dungeon>();
        
        this.depth = depth;
    }

    public void addDungeon(Dungeon d)
    {
        dungeonList.Add(d);
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
}
