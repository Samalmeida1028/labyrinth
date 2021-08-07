using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class TestingScript : MonoBehaviour
{
public GameObject wall;
private Pathfinder pathy;

//[SerializeField] private Follow follower;
private void Start(){
     pathy = new Pathfinder(10,10,transform.position);


}

private void Update(){
if(Input.GetMouseButtonDown(0)){
Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
Vector3 start = pathy.GetGrid().GetWorldPos(0,0);
pathy.GetGrid().GetXY(mousePos, out int x, out int y);
Vector3 end = new Vector3(x,y);
List<Vector3> path = pathy.FindPath(start,end);
if(path != null){
    for(int i = 0; i<path.Count-1; i++){
        Debug.DrawLine(new Vector3(path[i].x,path[i].y)*10f + Vector3.one *5f, new Vector3(path[i+1].x,path[i+1].y)*10f + Vector3.one*5f, Color.green,3f);
    }
}
//Debug.Log(mousePos);
//follower.SetTargetPosition(mousePos);


}

if(Input.GetMouseButtonDown(1)){
Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
Instantiate(wall,mousePos,Quaternion.identity);
pathy.GetGrid().GetXY(mousePos, out int x, out int y);
pathy.GetNode(x,y).SetIsWalkable(!pathy.GetNode(x,y).isWalkable);


}


}

}
