using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridGame : MonoBehaviour
{

    public GameObject Node;

    public GameObject[,] NodeArray;
    
    public int range;

    public Vector3 worldPos;

    public int cellSize;

    public float waitTime;

    public float time;

    public int arrayDimensions;
        // Start is called before the first frame update
    void Start()
    {
        worldPos = transform.position;
        NodeArray = new GameObject[arrayDimensions,arrayDimensions];

        for(int i = 0; i< arrayDimensions; i++){
            for(int j = 0; j < arrayDimensions;j++){
                NodeArray[i,j] = Node;
                Debug.Log(i+" and "+j +" are done.");


            }


        }




    }

    // Update is called once per frame
    void Update()
    {
        time+= Time.deltaTime;
        if(time>waitTime){



        }

        
    }

    public void Activate(){

        worldPos = transform.position;
        for(int i = 0; i< arrayDimensions; i++){
            for(int j = 0; j < arrayDimensions;j++){
                Instantiate(NodeArray[i,j],new Vector3((i+worldPos.x)*cellSize,(j+worldPos.y)*cellSize),Quaternion.identity);


            }


        }


    }
}
