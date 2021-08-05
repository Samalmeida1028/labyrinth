using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
public int x;
public int y;
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

public void CalculateFCost(){
    fCost = gCost + hCost;


}

public override string ToString(){
    return x + "," + y;
}

}
