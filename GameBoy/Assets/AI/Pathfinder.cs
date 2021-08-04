using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
private GridOBJ<Node> grid;
private List<Node> openList;
private List<Node> closeList;

public Pathfinder(int width, int height){
    grid = new GridOBJ<Node>(width,height, 1.25f, Vector3.zero,(GridOBJ<Node> g, int x,int y)=> new Node(g, x, y));

}

private  List<Node> FindPath(int startX, int startY, int endX, int endY){
    Node startNode = grid.GetGridOBJ(startX,startY);
    openList = new List<Node>();
    closeList = new List<Node>();


}


}
