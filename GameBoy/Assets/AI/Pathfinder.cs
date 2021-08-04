using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
private GridOBJ<Node> grid;

public Pathfinder(int width, int height){
    grid = new GridOBJ<Node>(width,height, 1.25f, Vector3.zero,(GridOBJ<Node> g, int x,int y)=> new Node(g, x, y));

}


}
