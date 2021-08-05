using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class TestingScript : MonoBehaviour
{

Pathfinder pathy;
private void Start(){
     pathy = new Pathfinder(8,8,this.transform.position);


}

private void Update(){
if(Input.GetMouseButtonDown(0)){
Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
pathy.GetGrid().GetXY(mousePos, out int x, out int y);
List<Node> path = pathy.FindPath(0,0,x,y);
if(path != null){
    for(int i = 0; i<path.Count; i++){
        Debug.DrawLine(new Vector3(path[i].x,path[i].y)*10f + Vector3.one *5f, new Vector3(path[i+1].x,path[i+1].y)*10f + Vector3.one, Color.green);
    }
}


}


}

}
