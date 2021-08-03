using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGenerator : MonoBehaviour
{
    public int numberOfRooms = 16;
    public float maxTreeLength;
    public int dungeonCount = 0;
    public int cols;
    public int rows;
    public GameObject wall; 
    public GameObject splitPrefab;
    public float tilePixelCount = 1.25f;
    public Vector4 color;
    //private float offSet = .2f;
    public int minRoomSize = 8;
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
        }
        
    }

    public void generateBoard()
    {
        
        GameObject blockHolder = new GameObject ("BlockHolder");
        for (float i = 0f; i<cols*tilePixelCount; i+=tilePixelCount)
        {
            for (float j =0f; j< rows*tilePixelCount; j+=tilePixelCount)
            {
                Instantiate(wall, new Vector3(i,j,0), Quaternion.identity, blockHolder.transform);
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
            
            /**if(!(checkSplitBoundary(yPos-y_Min)||checkSplitBoundary(y_Max-yPos))){
                bool canFit = false;
                while(canFit==false){
                    yPos = Random.Range(y_Min,y_Max);
                    canFit = (!(checkSplitBoundary(yPos-y_Min)||checkSplitBoundary(y_Max-yPos)));
                }    
            }**/

            yPos = yPos - yPos%.625f;
            

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

            /**if(!(checkSplitBoundary(xPos-x_Min)||checkSplitBoundary(x_Max-xPos))){
                bool canFit = false;
                while(canFit==false){
                    xPos = Random.Range(x_Min,x_Max);
                    canFit = (!(checkSplitBoundary(xPos-x_Min)||checkSplitBoundary(x_Max-xPos)));
                }    
            }**/

            xPos = xPos - xPos%.625f;

            
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
    public void generateDungeonTree(Dungeon d){

        if(d.depth<maxTreeLength){
            dungeonCount++;
            float x_Min = d.botLeft.x;
            float x_Max = d.topRight.x;
            float y_Min = d.botLeft.y;
            float y_Max = d.topRight.y;

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

            foreach(Dungeon dun in d.dungeonList){
                generateDungeonTree(dun);
            }
        }
    }

    public bool checkSplitBoundary(float val)
    {
        return (val>((minRoomSize+2)*tilePixelCount));
    }

}