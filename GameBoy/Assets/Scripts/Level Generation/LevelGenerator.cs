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
    private int dungeonCount = 0;
    public List<Dungeon> finalDungeonList = new List<Dungeon>();

    public GameObject wall; 
    public GameObject backgroundWall; 
    public GameObject collisionDetector;
    public GameObject roomCenterPrefab;

    public GameObject splitPrefab;
    public float tilePixelCount = 1.25f;
    public Vector4 color;
    public float roomBuffer = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        color = splitPrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        maxTreeLength = Mathf.Log(numberOfRooms,2);
        
    }

    void Update(){

        if(Input.GetKey(KeyCode.Space))
        {
           dungeonCount=0;
           generateBoard();
           generateDungeonTree(new Dungeon(new Vector2(0,0), new Vector2(cols*tilePixelCount,rows*tilePixelCount), 0));
           generateRooms();
        }
        Debug.Log(finalDungeonList.Count);
        
    }

    public void generateBoard()
    {
        
        GameObject blockHolder = new GameObject ("BlockHolder");
        for (float i = 0f; i<cols*tilePixelCount; i+=tilePixelCount)
        {
            for (float j =0f; j< rows*tilePixelCount; j+=tilePixelCount)
            {
                Instantiate(backgroundWall, new Vector3(j,i,0), Quaternion.identity, blockHolder.transform);
            }
        }
    }
    /**
    *   The split method splits the current dungeon into 2 randomly sized dungeons 
    *   within the input bounds
    **/
    public Split doSplit(float x_Min, float x_Max, float y_Min, float y_Max)
    {
        Vector4[] ColorList  = new Vector4[6];
        
        ColorList[0] = (Color.black);
        ColorList[1] = (Color.blue);
        ColorList[2] = (Color.gray);
        ColorList[3] = (Color.white);
        ColorList[4] = (Color.green);
        ColorList[5] = (Color.yellow);

        int k = 0;
        if(dungeonCount-1<6){
            k = dungeonCount-1;
        }else{
            k=5;
        }
        
        color = ColorList[k];
        if(dungeonCount == 15){
            color = Color.red;
        }
        splitPrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;

        float width = x_Max - x_Min;
        float height = y_Max - y_Min;

        float xPos = 0;
        float yPos = 0;

        bool horizontal;

        if(dungeonCount==1)
        {
            horizontal = (Random.Range(0,2)==1);


        }else{
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
            

            for (float i = x_Min; i<x_Max;i+=tilePixelCount)
            {
                Instantiate(splitPrefab, new Vector3(i,yPos,0), Quaternion.identity);
            }
            
        }
        else
        {
            x_Min+=((.45f/Mathf.Sqrt(dungeonCount+1))*cols*tilePixelCount);
            x_Max-=((.45f/Mathf.Sqrt(dungeonCount+1))*cols*tilePixelCount);

            xPos = Random.Range((x_Min+(minRoomSize+2)*tilePixelCount),(x_Max-(minRoomSize+2)*tilePixelCount));

            xPos = round(xPos);

            
            for (float i = y_Min; i<y_Max;i+=tilePixelCount)
            {
                Instantiate(splitPrefab, new Vector3(xPos,i,0), Quaternion.identity);
            }
            
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
                d.addDungeon(new Dungeon(new Vector2(x_Min, newSplit.y), d.topRight,d.depth+1));
                //Bottom Dungeon
                d.addDungeon(new Dungeon(d.botLeft, new Vector2(x_Max, newSplit.y), d.depth+1));
            }
            else
            {
                //Left Dungeon
                d.addDungeon(new Dungeon(d.botLeft, new Vector2(newSplit.x, y_Max),d.depth+1));
                //Right Dungeon
                d.addDungeon(new Dungeon(new Vector2(newSplit.x, y_Min), d.topRight,d.depth+1));
            }

            foreach(Dungeon dun in d.dungeonList)
            {
                dun.setParent(d);
                generateDungeonTree(dun);

                if(dun.depth==maxTreeLength)
                {
                    finalDungeonList.Add(dun);
                }
            }
        }
    }

    public bool checkSplitBoundary(float val)
    {
        return (val>((minRoomSize+roomBuffer)*tilePixelCount));
    }

    public void generateRooms()
    {
        int count = 0;
        foreach(Dungeon dungeon in finalDungeonList)
        {   
            count++;
            if(checkSplitBoundary(dungeon.width) && checkSplitBoundary(dungeon.height))
            {
                float roomBuffer = 5f;

                float roomWidth = Random.Range((minRoomSize+roomBuffer)*tilePixelCount,((dungeon.width<(maxRoomSize+1)*tilePixelCount) ? dungeon.width:(maxRoomSize+1)*tilePixelCount));
                float roomHeight = Random.Range((minRoomSize+roomBuffer)*tilePixelCount,((dungeon.height<(maxRoomSize+1)*tilePixelCount) ? dungeon.height:(maxRoomSize+1)*tilePixelCount));
                
                float x_Min = dungeon.botLeft.x;
                float x_Max = dungeon.topRight.x;
                float y_Min = dungeon.botLeft.y;
                float y_Max = dungeon.topRight.y;
        
                float x_Center = round((x_Min + (x_Max-x_Min)/2));
                float y_Center = round((y_Min + (y_Max-y_Min)/2));
                Vector2 dungeonCenter = new Vector2(x_Center,y_Center);
                
                Debug.Log(count + ": " + dungeonCenter);

                bool right = Random.Range(0,2)==1;
                bool up = Random.Range(0,2)==1;

                float x_offSet = (right ? Random.Range(dungeonCenter.x+roomWidth/2,x_Max)-(dungeonCenter.x+roomWidth/2) : -(Random.Range(x_Min,dungeonCenter.x-roomWidth/2)-(x_Min)));
                float y_offSet = (up ? Random.Range(dungeonCenter.y+roomHeight/2,y_Max)-(dungeonCenter.y+roomHeight/2) : -(Random.Range(y_Min,dungeonCenter.y-roomHeight/2)-(y_Min)));

                Vector2 roomCenter = new Vector2(round(dungeonCenter.x + x_offSet),round(dungeonCenter.y + y_offSet));

                Debug.Log(count + ": " + roomCenter);

                float roomX_Min = round((roomCenter.x-roomWidth/2)+roomBuffer);
                float roomY_Min = round((roomCenter.y-roomHeight/2)+roomBuffer);
                float roomX_Max = round((roomCenter.x+roomWidth/2)-roomBuffer);
                float roomY_Max = round((roomCenter.y+roomHeight/2)-roomBuffer);
                
                Debug.Log(count + ": Width " + roomWidth);
                Debug.Log(count + ": Height " + roomHeight);
                Debug.Log("Room Bottom Left " + count + ": " + new Vector2(roomX_Min, roomY_Min));
                Debug.Log("Room Bottom Right " + count + ": " + new Vector2(roomX_Max, roomY_Max));

                GameObject roomCenter_ = roomCenterPrefab;
                roomCenter_.GetComponent<Transform>().GetChild(0).GetComponent<Room>().roomNumber=count;
                Instantiate(roomCenter_, new Vector3(roomCenter.x,roomCenter.y, 0), Quaternion.identity);

                for (float i = roomY_Min; i<=roomY_Max; i+=tilePixelCount)
                {
                    
                    for (float j = roomX_Min; j<=roomX_Max; j+=tilePixelCount)
                    {

                        collisionDetector.GetComponent<Transform>().position = new Vector3(j,i,0);
                        
                        if(i==roomY_Min||i>=roomY_Max||j==roomX_Min||j>=roomX_Max)
                        {
                            Instantiate(wall, new Vector3(j,i,0), Quaternion.identity);
                            Debug.Log("i= " + (i-roomY_Min) + "Room Height: " + roomHeight);
                            Debug.Log("j= " + (j-roomX_Min) + "Room Width: " + roomWidth);
                        }

                    }
                }
            }
            else
            {
                dungeon.failSplit = true;
            }

        }
    }

    public float round(float f)
    {
        return (f - (f%(tilePixelCount)));
    }   
}