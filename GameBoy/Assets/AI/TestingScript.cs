using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class TestingScript : MonoBehaviour
{
    private GridOBJ grid;
    // Start is called before the first frame update
    void Start()
    {

        grid = new GridOBJ(128,128,1.25f,this.transform.position);
        
    }
    private void Update(){
        if(Input.GetMouseButtonDown(0)){
            UtilsClass.GetMouseWorldPosition();
            grid.SetVal(UtilsClass.GetMouseWorldPosition(),1);

        }
        if(Input.GetMouseButtonDown(1)){
            Debug.Log(grid.GetVal(UtilsClass.GetMouseWorldPosition()));

        }


    }
}
