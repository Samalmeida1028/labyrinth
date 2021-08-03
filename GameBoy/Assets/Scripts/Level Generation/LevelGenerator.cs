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
    private float offSet = .5f;
    public int minRoomSize = 8;
    // Start is called before the first frame update
    void Start()
    {
        maxTreeLength = Mathf.Log(numberOfRooms,2);
        //generateDungeonTree(new Dungeon(new Vector2(0,0), new Vector2(cols*tilePixelCount,rows*tilePixelCount)));
        
    }

    void Update(){

        if(Input.GetKey(KeyCode.Space)){
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
        float xMin = x_Min + (x_Min*offSet);
        float xMax = x_Max - (x_Max*offSet);
        float yMin = y_Min + (y_Min*offSet);
        float yMax = y_Max - (y_Max*offSet);

        if(dungeonCount==1)
        {
            xMin+=(.5f*cols*tilePixelCount);
            yMin+=(.5f*rows*tilePixelCount);
        }

        bool horizontal = (Random.Range(0,2)==1);
        float xPos = 0;
        float yPos = 0;

        if(checkSplitBoundary(y_Max-y_Min))
        {
            horizontal = false;
        }
        else if(checkSplitBoundary(x_Max-x_Min))
        {
            horizontal = true;
        }

        if(horizontal)
        {
            yPos = Random.Range(yMin,yMax);

            if(!checkSplitBoundary(yPos)){
                bool canFit = false;
                while(canFit==false){
                    yPos = Random.Range(yMin,yMax);
                    canFit = checkSplitBoundary(yPos);
                }    
            }

            yPos = yPos - yPos%.625f;

            for (float i = x_Min; i<x_Max;i+=tilePixelCount)
            {
                Instantiate(splitPrefab, new Vector3(i,yPos,0), Quaternion.identity);
            }
        }
        else
        {
            xPos = Random.Range(xMin,xMax);

            if(!checkSplitBoundary(xPos)){
                bool canFit = false;
                while(canFit==false){
                    xPos = Random.Range(xMin,xMax);
                    canFit = checkSplitBoundary(xPos);
                }    
            }

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
            float xMin = d.botLeft.x;
            float xMax = d.topRight.x;
            float yMin = d.botLeft.y;
            float yMax = d.topRight.y;

            Split newSplit = d.setSplit(doSplit(xMin, xMax, yMin, yMax));

            if(newSplit.horizontal)
            {             
                //Top Dungeon
                d.addDungeon(new Dungeon(new Vector2(xMin, newSplit.y), d.topRight,d.depth+1));
                //Bottom Dungeon
                d.addDungeon(new Dungeon(d.botLeft, new Vector2(xMax, newSplit.y), d.depth+1));
            }
            else
            {
                //Left Dungeon
                d.addDungeon(new Dungeon(d.botLeft, new Vector2(newSplit.x, yMax),d.depth+1));
                //Right Dungeon
                d.addDungeon(new Dungeon(new Vector2(newSplit.x, yMin), d.topRight,d.depth+1));
            }

            foreach(Dungeon dun in d.dungeonList){
                generateDungeonTree(dun);
            }
        }
    }

    public bool checkSplitBoundary(float val)
    {
        return val<(minRoomSize+2)*tilePixelCount;
    }
    
}