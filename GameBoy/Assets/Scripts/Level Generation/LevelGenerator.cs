using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Dimensions")]
    public int numberOfRooms = 16;
    public int cols;
    public int rows;
    public int minRoomSize = 8;
    public int maxRoomSize = 24;

    [Header("Object Prefabs")]
    public GameObject wall;  
    public GameObject roomCenterPrefab;

    [Header("Tile Prefabs")]
    //public Tile wallTile;

    [Header("Counter Variables")]
    private float maxTreeLength;
    private int dungeonCount = 0;
    private int roomCount = 0;

    [Header("Grid Settings")]
    public float tilePixelCount = 1.25f;
    private float roomBuffer;

    [Header("Tilemaps")]
    public Grid foregroundGrid;
    public TileBase wallTile;
    public TileBase floorTile;
    public Tilemap foregroundTiles;
    public Tilemap backgroundTiles;

    //The starting Dungeon
    private Dungeon startDungeon;

    //The grid that holds all the impassable objects
    public int [,] grid;

    void Start()
    {   
        foregroundGrid.GetComponent<Transform>().localScale = new Vector3(tilePixelCount,tilePixelCount,1);
        foregroundTiles = foregroundGrid.GetComponent<Transform>().GetChild(0).gameObject.GetComponent<Tilemap>();
        backgroundTiles = foregroundGrid.GetComponent<Transform>().GetChild(1).gameObject.GetComponent<Tilemap>();

        roomBuffer = 2*tilePixelCount;
        //Instantiate the root of the Binary Dungeon Tree using the Level Dimensions variables
        startDungeon = new Dungeon(new Vector2(0,0), new Vector2(cols*tilePixelCount,rows*tilePixelCount), 0);
        //Calculate the maximum length of the Binary Dungeon Tree based off the number of rooms
        maxTreeLength = Mathf.Log(numberOfRooms,2);

        grid = new int [rows,cols];

        //First generate the grid of wall objects
        generateBoard();
        //Then generate the Binary Tree and partition the grid into randomly sized parts equal to the number of rooms
        generateDungeonTree(startDungeon);
        //Lastly generate the rooms in the grid partitions and connect them with hallways
        generateRoom(startDungeon);
    }

    void Update()
    {

    }

    /**
    *   Generates a cols*rows sized board of walls and stores them in a grid of game objects
    **/
    public void generateBoard()
    {
        
        //GameObject blockHolder = new GameObject ("BlockHolder");
        for (int i = 0; i<cols; i++)
        {
            for (int j = 0; j< rows; j++)
            {
                grid[j,i] = 1;
                foregroundTiles.SetTile(foregroundGrid.WorldToCell(new Vector3(j*tilePixelCount,i*tilePixelCount,0)),wallTile);
  
            }
        }
    }
    /**
    *   The split method splits the current dungeon into 2 randomly sized dungeons 
    *   within the input bounds
    **/
    public Split doSplit(float x_Min, float x_Max, float y_Min, float y_Max)
    {

        float width = x_Max - x_Min;
        float height = y_Max - y_Min;

        float xPos = 0;
        float yPos = 0;

        bool horizontal;

        if(dungeonCount==1)
        {
            horizontal = (Random.Range(0,2)==1);


        }
        else
        {
            if((width>height)&&(width/height>=1.25))
            {
                horizontal = false;
            }
            else if((height>width)&&(height/width>=1.25))
            {
                horizontal = true;
            }
            else
            {
                horizontal = (Random.Range(0,2)==1);
            }
        }

        if(horizontal)
        {

            y_Min+=((.45f/Mathf.Sqrt(dungeonCount+1))*rows*tilePixelCount);
            y_Max-=((.45f/Mathf.Sqrt(dungeonCount+1))*rows*tilePixelCount);

            yPos = Random.Range((y_Min + (minRoomSize+2)*tilePixelCount),(y_Max - (minRoomSize+2)*tilePixelCount));

            yPos = round(yPos);

        }
        else
        {
            x_Min+=((.45f/Mathf.Sqrt(dungeonCount+1))*cols*tilePixelCount);
            x_Max-=((.45f/Mathf.Sqrt(dungeonCount+1))*cols*tilePixelCount);

            xPos = Random.Range((x_Min+(minRoomSize+2)*tilePixelCount),(x_Max-(minRoomSize+2)*tilePixelCount));

            xPos = round(xPos);

        }

        return new Split(xPos,yPos,horizontal);
    }

    /**
    *   The generateDungeonTree method recursively generates a binary tree of "dungeons" until the dungeon depth is reached
    *   on each recursive call the root dungeon is split into two subdungeons using the doSplit method
    *   a dungeon is a datatype which holds information on a randomly sized space in which a room can be generated
    *   see Dungeon class for more details
    **/
    public void generateDungeonTree(Dungeon d)
    {
        Dungeon lDungeon;
        Dungeon rDungeon;
        if(d.depth<maxTreeLength)
        {
            dungeonCount++;
            float x_Min = round(d.botLeft.x);
            float x_Max = round(d.topRight.x);
            float y_Min = round(d.botLeft.y);
            float y_Max = round(d.topRight.y);

            Split newSplit = d.setSplit(doSplit(x_Min, x_Max, y_Min, y_Max));

            if(newSplit.horizontal)
            {             
                //Top Dungeon
                lDungeon = (new Dungeon(new Vector2(x_Min, newSplit.y), d.topRight,d.depth+1));
                //Bottom Dungeon
                rDungeon = (new Dungeon(d.botLeft, new Vector2(x_Max, newSplit.y), d.depth+1));
            }
            else
            {
                //Left Dungeon
                lDungeon = (new Dungeon(d.botLeft, new Vector2(newSplit.x, y_Max),d.depth+1));
                //Right Dungeon
                rDungeon = (new Dungeon(new Vector2(newSplit.x, y_Min), d.topRight,d.depth+1));
            }

            //Set the parent and children of the dungeons
            lDungeon.setParent(d);
            rDungeon.setParent(d);
            d.setLeftDungeon(lDungeon);
            d.setRightDungeon(rDungeon);

            //make a recursive call on each subdungeon
            generateDungeonTree(lDungeon);
            generateDungeonTree(rDungeon);

        }
    }

    /**
    *   Generates a Room datatype that fits within the bounds of the given split
    *   Once all possible rooms are generated, the generateHall method is called to connect the rooms
    **/
    public void generateRoom(Dungeon dungeon)
    {
        if(dungeon.leftDungeon!=null||dungeon.rightDungeon!=null)
        {
            if(dungeon.leftDungeon!=null)
            {   
                generateRoom(dungeon.leftDungeon);
            }
            if(dungeon.rightDungeon!=null)
            {
                generateRoom(dungeon.rightDungeon);
            }

            if(dungeon.leftDungeon!=null&&dungeon.rightDungeon!=null)
            {
                generateHall(dungeon.leftDungeon,dungeon.rightDungeon);
            }
        }
        else
        {
            roomCount++;
            if(checkSplitBoundary(dungeon.width) && checkSplitBoundary(dungeon.height))
            {
                float roomBuffer = 5f;

                float roomWidth = round(Random.Range((minRoomSize+roomBuffer)*tilePixelCount,((dungeon.width<(maxRoomSize+1)*tilePixelCount) ? dungeon.width:(maxRoomSize+1)*tilePixelCount)));
                float roomHeight = round(Random.Range((minRoomSize+roomBuffer)*tilePixelCount,((dungeon.height<(maxRoomSize+1)*tilePixelCount) ? dungeon.height:(maxRoomSize+1)*tilePixelCount)));
                
                float x_Min = dungeon.botLeft.x;
                float x_Max = dungeon.topRight.x;
                float y_Min = dungeon.botLeft.y;
                float y_Max = dungeon.topRight.y;
        
                float x_Center = round((x_Min + (x_Max-x_Min)/2));
                float y_Center = round((y_Min + (y_Max-y_Min)/2));
                Vector2 dungeonCenter = new Vector2(x_Center,y_Center);

                bool right = Random.Range(0,2)==1;
                bool up = Random.Range(0,2)==1;

                float x_offSet = (right ? Random.Range(dungeonCenter.x+roomWidth/2,x_Max)-(dungeonCenter.x+roomWidth/2) : -(Random.Range(x_Min,dungeonCenter.x-roomWidth/2)-(x_Min)));
                float y_offSet = (up ? Random.Range(dungeonCenter.y+roomHeight/2,y_Max)-(dungeonCenter.y+roomHeight/2) : -(Random.Range(y_Min,dungeonCenter.y-roomHeight/2)-(y_Min)));

                Vector2 roomCenter = new Vector2((dungeonCenter.x + x_offSet),(dungeonCenter.y + y_offSet));

                float roomX_Min = round((roomCenter.x-roomWidth/2)+roomBuffer);
                float roomY_Min = round((roomCenter.y-roomHeight/2)+roomBuffer);
                float roomX_Max = round((roomCenter.x+roomWidth/2)-roomBuffer);
                float roomY_Max = round((roomCenter.y+roomHeight/2)-roomBuffer);

                float roomFinalWidth = roomX_Max-roomX_Min;
                float roomFinalHeight = roomY_Max-roomY_Min;
                Vector2 roomFinalCenter = new Vector2((roomFinalWidth/2 + roomX_Min)+tilePixelCount/2, (roomFinalHeight/2 + roomY_Min)+tilePixelCount/2);

                GameObject [,] roomGrid = new GameObject [(int)(roomFinalWidth/tilePixelCount)+1,(int)(roomFinalHeight/tilePixelCount)+1];
                GameObject roomObj = roomCenterPrefab;
                roomObj.GetComponent<Transform>().GetComponent<RoomObj>().setRoom(new Room(roomCount, roomFinalWidth, roomFinalHeight, new Vector2(roomX_Min, roomY_Min), new Vector2(roomX_Max, roomY_Max), roomFinalCenter));
                roomObj.GetComponent<Transform>().GetComponent<RoomObj>().setGrid(roomGrid);
                dungeon.setRoom(new Room(roomCount, roomFinalWidth, roomFinalHeight, new Vector2(roomX_Min, roomY_Min), new Vector2(roomX_Max, roomY_Max), roomFinalCenter));
                GameObject roomHolder = new GameObject ("Room " + dungeon.room.roomNumber);
                Instantiate(roomObj, new Vector3(roomFinalCenter.x,roomFinalCenter.y, 0), Quaternion.identity, roomHolder.transform);

                drawRoom(new Vector2(roomX_Min,roomY_Min),new Vector2(roomX_Max,roomY_Max));

            }
            else
            {
                dungeon.failSplit = true;
            }
        }
        
        
    }

    /**
    *   Connects all the rooms with a hall by first connecting each sister leaf at the bottom of the Binary tree, 
    *   then moving up the tree and connecting those sisters
    **/
    public void generateHall(Dungeon lDungeon, Dungeon rDungeon)
    {
        
        if(!(lDungeon.failSplit||rDungeon.failSplit))
        { 
            Room lRoom = lDungeon.getRoom();
            Room rRoom = rDungeon.getRoom();


            Vector2 dirVector = lRoom.roomCenter-rRoom.roomCenter;
            int side = 0;
               
            /**
            *   Calculate which side of the room to create the corridor on
            **/
            if(Mathf.Abs(dirVector.x)>Mathf.Abs(dirVector.y))
            {
                
                //horizontal
                if(dirVector.x>0)
                {
                    //Left Side
                    side = 0;
                }
                else
                {
                    //Right Side
                    side = 2;
                }
            }
            else
            {
                //vertical
                if(dirVector.y<0)
                {
                    //Top Side
                    side = 1;
                }
                else
                {
                    //Bottom Side
                    side = 3;
                }

            }

            Vector2 vertRange = new Vector2(0,0);
            Vector2 horzRange = new Vector2(0,0);

            Vector2 startPos = new Vector2(0,0);
            Vector2 meetPos = new Vector2(0,0); 
            Vector2 endPos = new Vector2(0,0);
            
            //Calculate whether or not the hall will have to make a turn in order to reach the desired room
            bool turn = ((Mathf.Abs(dirVector.x) - (lRoom.roomDimensions.width/2 + rRoom.roomDimensions.width/2)) >=0 && (Mathf.Abs(dirVector.y) - (lRoom.roomDimensions.height/2 + rRoom.roomDimensions.height/2)) >=0 );

            //If the hall must turn, three points must be randomly generated, the start point, meet point and end point
            if(turn)
            {
                
                if(side == 0||side ==2)
                {
                    vertRange = new Vector2(lRoom.roomDimensions.yMin,lRoom.roomDimensions.yMax);
                    horzRange = new Vector2(rRoom.roomDimensions.xMin,rRoom.roomDimensions.xMax);

                    startPos = new Vector2((side==0 ? lRoom.roomDimensions.xMin : lRoom.roomDimensions.xMax),round(Random.Range(vertRange.x+tilePixelCount*2,vertRange.y-tilePixelCount)));
                    meetPos = new Vector2(round(Random.Range(horzRange.x+tilePixelCount*2,horzRange.y-tilePixelCount)),startPos.y);
                    endPos = new Vector2(meetPos.x,(dirVector.y>0 ? rRoom.roomDimensions.yMax : rRoom.roomDimensions.yMin));
                }
                else if (side ==1||side ==3)
                {
                    vertRange = new Vector2(rRoom.roomDimensions.yMin,rRoom.roomDimensions.yMax);
                    horzRange = new Vector2(lRoom.roomDimensions.xMin,lRoom.roomDimensions.xMax);
                    startPos = new Vector2(round(Random.Range(horzRange.x+tilePixelCount*2,horzRange.y-tilePixelCount)),(side==1 ? lRoom.roomDimensions.yMax : lRoom.roomDimensions.yMin));
                    meetPos = new Vector2(startPos.x,round(Random.Range(vertRange.x+tilePixelCount*2,vertRange.y-tilePixelCount)));
                    endPos = new Vector2((dirVector.x>0 ? rRoom.roomDimensions.xMax : rRoom.roomDimensions.xMin),meetPos.y);
                }
                drawHall(startPos,meetPos);
                drawHall(meetPos,endPos);
            }
            //If the hall doesn't have to turn only a start and end point are reandomly generated
            else
            {

                if(side == 0||side ==2)
                {
                    vertRange = new Vector2((lRoom.botLeft.y>rRoom.botLeft.y ? lRoom.botLeft.y : rRoom.botLeft.y),(lRoom.topRight.y>rRoom.topRight.y ? rRoom.topRight.y : lRoom.topRight.y));
                    startPos = new Vector2((side==0 ? lRoom.roomDimensions.xMin : lRoom.roomDimensions.xMax),round(Random.Range(vertRange.x+tilePixelCount*2,vertRange.y-tilePixelCount)));
                    endPos = new Vector2((side==0 ? rRoom.roomDimensions.xMax : rRoom.roomDimensions.xMin),startPos.y);
                }
                else if (side ==1||side ==3)
                {
                    horzRange = new Vector2((lRoom.botLeft.x>rRoom.botLeft.x ? lRoom.botLeft.x : rRoom.botLeft.x),(lRoom.topRight.x>rRoom.topRight.x ? rRoom.topRight.x : lRoom.topRight.x));
                    startPos = new Vector2(round(Random.Range(horzRange.x+tilePixelCount*2,horzRange.y-tilePixelCount)),(side==1 ? lRoom.roomDimensions.yMax : lRoom.roomDimensions.yMin));
                    endPos = new Vector2(startPos.x, (side==1 ? rRoom.roomDimensions.yMin : rRoom.roomDimensions.yMax));
                }
                drawHall(startPos,endPos);
            }
 
        }
    }

    /**
    *   Carves a room from the grid of walls
    **/
    public void drawRoom(Vector2 min, Vector2 max)
    {
        for (float i =min.y; i<=max.y; i+=tilePixelCount)
        {
            
            for (float j = min.x; j<=max.x; j+=tilePixelCount)
            {   
                grid[(int)(j/tilePixelCount),(int)(i/tilePixelCount)]=0;
                foregroundTiles.SetTile(foregroundGrid.WorldToCell(new Vector3(j,i,0)),null);
                backgroundTiles.SetTile(foregroundGrid.WorldToCell(new Vector3(j,i,0)),floorTile);

            }
        }
    }

    /**
    *   Carves a hallway from the grid of walls
    **/
    public void drawHall(Vector2 startPos, Vector2 endPos)
    {
        if(startPos.y>endPos.y||startPos.x>endPos.x)
        {
            Vector2 temp = new Vector2(0,0);
            temp = endPos;
            endPos= startPos;
            startPos = temp;
        }
        for (float i =startPos.y; i<=endPos.y; i+=tilePixelCount)
        {
            
            for (float j = startPos.x; j<=endPos.x; j+=tilePixelCount)
            {   
                grid[(int)(j/tilePixelCount),(int)(i/tilePixelCount)]=0;
                foregroundTiles.SetTile(foregroundGrid.WorldToCell(new Vector3(j,i,0)),null);
                backgroundTiles.SetTile(foregroundGrid.WorldToCell(new Vector3(j,i,0)),floorTile);

            }
        }
        
    }

    /**
    *   Rounds the given float to the nearest tile
    *   useful for making sure objects snap to the grid
    **/
    public float round(float f)
    {
        return (f - (f%(tilePixelCount)));
    }   

    /**
    *   Calculates if a value will fit within it's split
    **/
    public bool checkSplitBoundary(float val)
    {
        return (val>((minRoomSize+roomBuffer)*tilePixelCount));
    }
    
}