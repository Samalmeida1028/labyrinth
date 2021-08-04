using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
private int x;
private int y;
private GridOBJ<Node> grid;

public int gCost;
public int hCost;
public int fCost;

public Node cameFromNode;
public Node(GridOBJ<Node> grid,int x, int y){
    this.grid = grid;
    this.x = x;
    this.y = y;



}

public override string ToString(){
    return x + "," + y;
}

}
