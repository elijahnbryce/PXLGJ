using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapManager : MonoBehaviour
{
    public static MapManager _Instance;

    [SerializeField] public Tilemap map;

    [SerializeField] private List<TileData> tileDataList;
    private Dictionary<Vector3Int, PathNode> tileDict;

    //private Pathfinding testfind;
    public BoundsInt bounds;

    private void Awake()
    {
        _Instance = this;
        GeneratePathGrid();

        Debug.Log(map.HasTile(new Vector3Int(bounds.max.x-1, bounds.max.y-1, 0)));
        Debug.Log(map.HasTile(new Vector3Int(bounds.min.x, bounds.min.y, 0)));
    }

    public void GeneratePathGrid()
    {
        tileDict = new Dictionary<Vector3Int, PathNode>();
        bounds = map.cellBounds;
        Debug.Log("bounds " + bounds.min.y + " " + bounds.max.y + " " + bounds.min.x + " " + bounds.max.x);
        // looping through all tiles
        for (int y = bounds.min.y, y1 = bounds.max.y; y < y1; y++)
        {
            for (int x = bounds.min.x, x1 = bounds.max.x; x < x1; x++)
            {
                Vector3Int tileLocation = new Vector3Int(x, y, 0);
                if (map.HasTile(tileLocation))
                {
                    tileDict.Add(tileLocation, new PathNode(x, y));
                }
            }
        }
    }

    private void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector3Int gridPos = map.WorldToCell(mousePos);

        //    TileBase clickedTile = map.GetTile(gridPos);
        //    bool walkableTile = tileDict[gridPos].isWalkable;
        //    Debug.Log("At position " + gridPos + "there is a " + clickedTile + "with isWalkable: " + walkableTile);
        //    List<PathNode> testNoodles = testfind.FindPath(0, 0, gridPos.x, gridPos.y);
        //    DrawPath(testNoodles);
        //    testfind.ClearPath();
        //}
    }

    public void DrawPath(List<PathNode> noodles)
    {
        if (noodles != null)
        {
            for (int i = 0; i < noodles.Count - 1; ++i)
            {
                Vector3 sp = map.CellToWorld(new Vector3Int(noodles[i].x, noodles[i].y, 0));
                Vector3 sp2 = map.CellToWorld(new Vector3Int(noodles[i + 1].x, noodles[i + 1].y, 0));
                Debug.DrawLine(sp, sp2, Color.red, 10f, false);
                //Debug.Log(testNoodles[i].x + "," + testNoodles[i].y);
            }
        }
    }

    private void Start()
    {
        // Testing
        //testfind = new Pathfinding();
    }

    public Dictionary<Vector3Int, PathNode> GetTilesDict()
    {
        Dictionary<Vector3Int, PathNode> copy_d = new Dictionary <Vector3Int, PathNode>();
        foreach (KeyValuePair<Vector3Int, PathNode> kv in tileDict)
        {
            PathNode noodle = new PathNode(kv.Value.x, kv.Value.y);
            noodle.isWalkable = kv.Value.isWalkable;
            noodle.gCost = int.MaxValue;
            noodle.CalcF();
            noodle.searchedFrom = null;
            copy_d.Add(kv.Key, noodle);
        }
        return copy_d;
    }

    public void SetTileUnwalkable(Vector3 cords, bool set=false)
    {
        Vector2Int[] blockDirs = { Vector2Int.zero, Vector2Int.left, -Vector2Int.one, Vector2Int.down };
        Vector3Int gridPos = map.WorldToCell(cords);
        foreach (Vector2Int dir in blockDirs)
        {
            Vector3Int checkPos = gridPos + new Vector3Int(dir.x, dir.y, 0);
            if (map.HasTile(checkPos))
            {
                PathNode targ = tileDict[checkPos];
                targ.isWalkable = set;
            }
        }
    }

    public Vector3 GetWorldPos(PathNode nood)
    {
        return map.CellToWorld(new Vector3Int(nood.x, nood.y, 0));
    }

    public Vector3Int GetGridPos(Vector3 coords) 
    {
        return map.WorldToCell(coords);
    }

    public bool CheckMap(Vector3Int loc)
    {
        return map.HasTile(loc);
    }
}
