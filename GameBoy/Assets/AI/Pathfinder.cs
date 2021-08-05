using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{

    private const int MOVE_STCOST = 10;
    private const int MOVE_DCOST = 14;
    private GridOBJ<Node> grid;
    private List<Node> openList;
    private List<Node> closeList;

    public Pathfinder(int width, int height, Vector3 origin)
    {
        grid = new GridOBJ<Node>(width, height, 1.25f, Vector3.zero, (GridOBJ<Node> g, int x, int y) => new Node(g, x, y));

    }

    public List<Node> FindPath(int startX, int startY, int endX, int endY)
    {
        Node startNode = grid.GetGridOBJ(startX, startY);
        Node endNode = grid.GetGridOBJ(endX, endY);

        openList = new List<Node> { startNode };
        closeList = new List<Node>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Node pathNode = grid.GetGridOBJ(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;


            }



        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestCostNode(openList);
            if (currentNode == endNode)
            {
                return (CalculatePath(endNode));

            }
            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach (Node neighborNode in GetNeighborList(currentNode))
            {
                if (closeList.Contains(neighborNode))
                {
                    continue;
                }

                int tentGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode);

                if (tentGCost < neighborNode.gCost)
                {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if(!openList.Contains(neighborNode)){
                        openList.Add(neighborNode);
                    }

                }



            }
        }


        //out of Nodes in OpenList

        return null;

    }

    private List<Node> GetNeighborList(Node currentNode)
    {
        List<Node> neighborList = new List<Node>();
        if (currentNode.x - 1 >= 0)//Left
        {
            neighborList.Add(GetNode(currentNode.x - 1, currentNode.y));

            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));//Left Down

            if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));//Left Up
        }
        if (currentNode.x + 1 < grid.GetWidth())
        {
            neighborList.Add(GetNode(currentNode.x = 1, currentNode.y));//Right

            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));//Right Down

            if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));//Right Up
        }
        if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x, currentNode.y - 1));//Down

        if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x, currentNode.y + 1));//Up

        return neighborList;

    }

    private Node GetNode(int x, int y)
    {
        return grid.GetGridOBJ(x, y);
    }

    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while(currentNode.cameFromNode != null){
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }

        path.Reverse();
        return path;



    }

    private int CalculateDistanceCost(Node a, Node b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remainder = Mathf.Abs(xDistance - yDistance);
        return MOVE_DCOST * Mathf.Min(xDistance, yDistance) + MOVE_STCOST * remainder;

    }

    private Node GetLowestCostNode(List<Node> pathNodeList)
    {
        Node lowestFCostNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;


    }


public GridOBJ<Node> GetGrid(){

return grid;

}

}
