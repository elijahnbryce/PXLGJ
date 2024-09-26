using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding
{
    private static MapManager MM = MapManager._Instance;
    private static GameManager gm = GameManager._Instance;
    private Tilemap tm = MM.map;
    private Dictionary<Vector3Int, PathNode> grid;
    private BoundsInt bounds;

    private List<PathNode> openList;
    private List<PathNode> closedList;

    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;


    public Pathfinding()
    {
        grid = MM.GetTilesDict();
        bounds = tm.cellBounds;
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = GetNode(startX, startY);
        PathNode endNode = GetNode(endX, endY);

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalcF();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCost(openList);
            if (currentNode == endNode)
            {
                // end case
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbor in GetNeightbours(currentNode))
            {
                if (closedList.Contains(neighbor)) continue;

                int tempGCost = currentNode.gCost + CalculateDistance(currentNode, neighbor);
                if (tempGCost < neighbor.gCost)
                {
                    neighbor.searchedFrom = currentNode;
                    neighbor.gCost = tempGCost;
                    neighbor.hCost = CalculateDistance(neighbor, endNode);
                    neighbor.CalcF();

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }   
        }
        // failure
        return null;
    }

    public List<Vector3> FindPathVectors(int startX, int startY, int endX, int endY)
    {
        List<PathNode> noodles = FindPath(startX, startY, endX, endY);
        List<Vector3> cookedNoodles = new List<Vector3>();

        if (noodles == null) return null;
        MM.DrawPath(noodles);

        foreach (PathNode nood in noodles)
        {
            cookedNoodles.Add(MM.GetWorldPos(nood));
        }
        return cookedNoodles;
    }

    private List<PathNode> GetNeightbours(PathNode cNode)
    {
        List<PathNode> neighbors = new List<PathNode>();
        Vector2Int upLeft = new Vector2Int(-1, 1); 
        Vector2Int[] checkDirs = { upLeft, Vector2Int.left, -Vector2Int.one, Vector2Int.down, -upLeft, Vector2Int.right, Vector2Int.one, Vector2Int.up };
        //                         upleft       left            down left      down         down right     right           up right        up

        Vector3Int gridPos = new Vector3Int(cNode.x, cNode.y, 0);
        //Debug.Log("At position " + gridPos + "there is a tile with isWalkable: " + cNode.isWalkable);

        foreach (Vector2Int dir in checkDirs)
        {
            Vector3Int checkPos = gridPos + new Vector3Int(dir.x, dir.y, 0);
            if (tm.HasTile(checkPos))
            {
                PathNode tempNode = GetNode(checkPos.x, checkPos.y);
                if (!tempNode.isWalkable) continue; // closedList.Add(tempNode);
                neighbors.Add(tempNode);
                //Debug.Log("Tile " + gridPos + "has unchecked neigboring tile: " + checkPos);
            }
        }
        return neighbors;
    }


     private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> noodles = new List<PathNode> { endNode };
        PathNode cNode = endNode;
        while (cNode.searchedFrom != null)
        {
            cNode = cNode.searchedFrom;
            noodles.Add(cNode);
        }
        noodles.Reverse();
        Debug.Log("Path Found");
        return noodles;
    }

    private PathNode GetNode(int x, int y, int z = 0)
    {
        Vector3Int tileLocation;
        tileLocation = new Vector3Int(x, y, z);
        if (!tm.HasTile(tileLocation))
        {
            Debug.Log(tileLocation + " -> doesn't exist in tilemap");
            return null;
        }
        return grid[tileLocation];
    }

    private int CalculateDistance(PathNode a,  PathNode b)
    {
        // go diagonal until we can't
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int leftovers = Mathf.Abs(xDistance - yDistance);
        return DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + STRAIGHT_COST * leftovers;
    }

    private PathNode GetLowestFCost(List<PathNode> pathList)
    {
        // randomly choose not the lowest cost
        bool whiff = (Random.Range(0f, 1f) < 100 / Mathf.Pow(2, gm.difficultySlider) / 100);
        PathNode lowestFCost = pathList[0];
        if (whiff) 
        {
            for (int i = 1, length = pathList.Count; i < length; i++)
            {
                if (pathList[i].fCost > lowestFCost.fCost)
                {
                    lowestFCost = pathList[i];
                }
            }
        }
        else
        {
            for (int i = 1, length = pathList.Count; i < length; i++)
            {
                if (pathList[i].fCost < lowestFCost.fCost)
                {
                    if (Random.Range(0f, 1f) < 100 / Mathf.Pow(2, gm.difficultySlider) / 100) continue;
                    lowestFCost = pathList[i];
                }
            }
        }
        return lowestFCost;
    }

    public void ClearPath()
    {
        for (int y = bounds.min.y, y1 = bounds.max.y; y <= y1; y++)
        {
            for (int x = bounds.min.x, x1 = bounds.max.x; x <= x1; x++)
            {
                Vector3Int searchLocation = new Vector3Int(x, y, 0);
                if (tm.HasTile(searchLocation))
                {
                    PathNode pathNode = grid[searchLocation];
                    pathNode.gCost = int.MaxValue;
                    pathNode.CalcF();
                    pathNode.searchedFrom = null;
                }
            }
        }
        openList.Clear();
    }
}
