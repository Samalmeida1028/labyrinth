using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinder Instance { get; private set; }

    private GridOBJ<Node> grid;
    private List<Node> openList;
    private List<Node> closedList;


    public Pathfinder(int width, int height, Vector3 origin) {
        Instance = this;
        grid = new GridOBJ<Node>(width, height, 10f, origin, (GridOBJ<Node> g, int x, int y) => new Node(g, x, y));
    }

    public GridOBJ<Node> GetGrid() {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<Node> path = FindPath(startX, startY, endX, endY);
        if (path == null) {
            return null;
        } else {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (Node Node in path) {
                vectorPath.Add(new Vector3(Node.x, Node.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }

    public List<Node> FindPath(float X, float Y, int endX, int endY) {
        grid.GetXY(new Vector3(X,Y),out int startX, out int startY);

        Node startNode =grid.GetGridOBJ(startX,startY);
        Debug.Log(grid.GetWorldPos(startX,startY));
        Node endNode = grid.GetGridOBJ(endX, endY);
        Debug.Log(grid.GetWorldPos(endX,endY));

        if (startNode == null || endNode == null) {
            // Invalid Path
            return null;
        }

        openList = new List<Node> { startNode };
        closedList = new List<Node>();

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                Node Node = grid.GetGridOBJ(x, y);
                Node.gCost = 99999999;
                Node.CalculateFCost();
                Node.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        


        while (openList.Count > 0) {
            Node currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                // Reached final node
 
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighborNode in GetneighborList(currentNode)) {
                if (closedList.Contains(neighborNode)) continue;
                if(!neighborNode.isWalkable){
                     closedList.Add(neighborNode);
                     continue;
                }
                if (!neighborNode.isWalkable) {
                    closedList.Add(neighborNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighborNode);
                if (tentativeGCost < neighborNode.gCost) {
                    neighborNode.cameFromNode = currentNode;
                    neighborNode.gCost = tentativeGCost;
                    neighborNode.hCost = CalculateDistanceCost(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!openList.Contains(neighborNode)) {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    private List<Node> GetneighborList(Node currentNode) {
        List<Node> neighborList = new List<Node>();

        if (currentNode.x - 1 >= 0) {
            // Left
            neighborList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.GetWidth()) {
            // Right
            neighborList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighborList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighborList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighborList;
    }

    public Node GetNode(int x, int y) {
        return grid.GetGridOBJ(x, y);
    }

    private List<Node> CalculatePath(Node endNode) {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(Node a, Node b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private Node GetLowestFCostNode(List<Node> NodeList) {
        Node lowestFCostNode = NodeList[0];
        for (int i = 1; i < NodeList.Count; i++) {
            if (NodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = NodeList[i];
            }
        }
        return lowestFCostNode;
    }


}
