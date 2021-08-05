using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public int numberOfRooms = 16;
    public int cols;
    public int rows;
    public int minRoomSize = 8;
    public int maxRoomSize = 24;

    private float maxTreeLength;
    private int dungeonCount = 1;
    int roomCount = 0;
    public List<Dungeon> finalDungeonList = new List<Dungeon>();

    public GameObject wall; 
    public GameObject backgroundWall; 
    public GameObject roomCenterPrefab;

    public GameObject splitPrefab;
    public GameObject collisionDetector;
    public GameObject [,] grid;

    public float tilePixelCount = 1.25f;
    public float roomBuffer = 2.5f;
    Dungeon startDungeon;

    // Start is called before the first frame update
    void Start()
    {
        startDungeon = new Dungeon(new Vector2(0,0), new Vector2(cols*tilePixelCount,rows*tilePixelCount), 0);
        maxTreeLength = Mathf.Log(numberOfRooms,2);
        collisionDetector = Instantiate(collisionDetector,new Vector3(1000,1000,0),Quaternion.identity);
        grid = new GameObject [rows,cols];
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
           dungeonCount=0;
           //generateBoard();
           generateDungeonTree(startDungeon);
           generateRoom(startDungeon);
        }
        
    }

    public void generateBoard()
    {
        
        GameObject blockHolder = new GameObject ("BlockHolder");
        for (int i = 0; i<cols; i++)
        {
            for (int j = 0; j< rows; j++)
            {
                grid[j,i] = Instantiate(backgroundWall, new Vector3(j*tilePixelCount,i*tilePixelCount,0), Quaternion.identity, blockHolder.transform);
  
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
    *   The generateDungeonTree method recursively generates a binary tree of "dungeons" until the dungeon counter is reached
    *   a dungeon is a datatype which holds information on a randomly sized space in which a room can be generated
    *   see Dungeon class for more details
    **/
    public void generateDungeonTree(Dungeon d)
    {
        Dungeon lDungeon;
        Dungeon rDungeon;
        //Debug.Log(dungeonCount);
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

            lDungeon.setParent(d);
            rDungeon.setParent(d);
            d.setLeftDungeon(lDungeon);
            d.setRightDungeon(rDungeon);
            generateDungeonTree(lDungeon);
            generateDungeonTree(rDungeon);

        }
    }

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
                
                for (float i = roomY_Min; i<=roomY_Max; i+=tilePixelCount)
                {
                    
                    for (float j = roomX_Min; j<=roomX_Max; j+=tilePixelCount)
                    {   
                        
                        Destroy(grid[(int)(j/tilePixelCount),(int)(i/tilePixelCount)]);

                        if(i==roomY_Min||i>=roomY_Max||j==roomX_Min||j>=roomX_Max)
                        {
                            roomGrid[(int)((j-roomX_Min)/tilePixelCount),(int)((i-roomY_Min)/tilePixelCount)] = Instantiate(wall, new Vector3(j,i,0), Quaternion.identity);
                        }

                    }
                }

                GameObject roomObj = roomCenterPrefab;
                roomObj.GetComponent<Transform>().GetComponent<RoomObj>().setRoom(new Room(roomCount, roomFinalWidth, roomFinalHeight, new Vector2(roomX_Min, roomY_Min), new Vector2(roomX_Max, roomY_Max), roomFinalCenter));
                roomObj.GetComponent<Transform>().GetComponent<RoomObj>().setGrid(roomGrid);
                dungeon.setRoom(new Room(roomCount, roomFinalWidth, roomFinalHeight, new Vector2(roomX_Min, roomY_Min), new Vector2(roomX_Max, roomY_Max), roomFinalCenter));
                //Debug.Log(dungeon.room.roomNumber);
                Instantiate(roomObj, new Vector3(roomFinalCenter.x,roomFinalCenter.y, 0), Quaternion.identity);

            }
            else
            {
                dungeon.failSplit = true;
                Debug.Log("Fail Gen");
            }
        }
        
        
    }

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
                if(dirVector.y>0)
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
            
            bool turn = ((Mathf.Abs(dirVector.x) - (lRoom.roomDimensions.width/2 + rRoom.roomDimensions.width/2)) <=0 || (Mathf.Abs(dirVector.y) - (lRoom.roomDimensions.height/2 + rRoom.roomDimensions.height/2)) <=0 );

            if(turn)
            {
                
                Debug.Log("Fuck You");
                /**if(side == 0||side ==2)
                {
                    vertRange = new Vector2(lRoom.roomDimensions.yMin,lRoom.roomDimensions.yMax);
                    horzRange = new Vector2(rRoom.roomDimensions.xMin,rRoom.roomDimensions.xMax);
                }
                else if (side ==1||side ==3)
                {
                    vertRange = new Vector2(rRoom.roomDimensions.yMin,rRoom.roomDimensions.yMax);
                    horzRange = new Vector2(lRoom.roomDimensions.xMin,lRoom.roomDimensions.xMax);
                }**/
            }
            else
            {

                if(side == 0||side ==2)
                {
                    vertRange = new Vector2((lRoom.topRight.y>rRoom.topRight.y ? rRoom.topRight.y : lRoom.topRight.y), (lRoom.botLeft.y>rRoom.botLeft.y ? rRoom.botLeft.y : lRoom.botLeft.y));
                    startPos = new Vector2((side==0 ? lRoom.roomDimensions.xMin : lRoom.roomDimensions.xMax),round(Random.Range(vertRange.x+tilePixelCount,vertRange.y-tilePixelCount)));
                    endPos = new Vector2((side==0 ? rRoom.roomDimensions.xMax : rRoom.roomDimensions.xMin),startPos.y);
                }
                else if (side ==1||side ==3)
                {
                    horzRange = new Vector2((lRoom.topRight.x>rRoom.topRight.x ? rRoom.topRight.x : lRoom.topRight.x), (lRoom.botLeft.x>rRoom.botLeft.x ? rRoom.botLeft.x : lRoom.botLeft.x));
                    startPos = new Vector2(round(Random.Range(horzRange.x+tilePixelCount,horzRange.y-tilePixelCount)),(side==1 ? lRoom.roomDimensions.yMax : lRoom.roomDimensions.yMin));
                    endPos = new Vector2(startPos.x, (side==1 ? rRoom.roomDimensions.yMin : rRoom.roomDimensions.yMax));
                }

            }

            for (float i = startPos.y; i<=endPos.y; i+=tilePixelCount)
            {
                
                for (float j = startPos.x; j<=endPos.x; j+=tilePixelCount)
                {   
                    
                    Destroy(grid[(int)(j/tilePixelCount),(int)(i/tilePixelCount)]);

                    Instantiate(wall, new Vector3(j,i,0), Quaternion.identity);


                }
            }
            Debug.Log("Room: " + lRoom.roomNumber + " Start: " + startPos + "End: " + endPos);
        }
    }

    public float round(float f)
    {
        return (f - (f%(tilePixelCount)));
    }   

    public bool checkSplitBoundary(float val)
    {
        return (val>((minRoomSize+roomBuffer)*tilePixelCount));
    }

}