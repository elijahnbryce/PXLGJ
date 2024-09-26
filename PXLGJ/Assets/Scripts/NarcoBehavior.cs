using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NarcoBehavior : MonoBehaviour
{
    public enum EnemyState
    {
        running,
        caught
    }

    public enum SpriteStatus
    {
        hiding,
        bubbling,
        showing
    }
    private static GameManager gm = GameManager._Instance;

    [Header("Movement")]
    [SerializeField] private SpriteStatus status;
    [SerializeField] private EnemyState state;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform escapeListParent;
    [SerializeField] private int pathIndex;
    private Transform closestEscape;
    private Vector3 closestTile;
    private Vector2 targDir;
    private Pathfinding NAVI;
    private IsoCRenderer icr;
    private List<Vector3> escapeNoodles;


    [Header("Sprite")]
    [SerializeField] private static float timeHidden = 7f, timeShown = 7f, bubbleTime = 3f;
    [SerializeField] private float[] statuses = { timeHidden, bubbleTime, timeShown };
    private string[] statusAnims = { "Hide", "Bubble", "Show" };
    private Animator anim;
    private SpriteRenderer sr;

    public float statusTime;
    public int statusIndex;

    private void Start()
    {
        anim = GetComponent<Animator>();
        NAVI = new Pathfinding();
        icr = GetComponentInChildren<IsoCRenderer>();
        sr = GetComponent<SpriteRenderer>();
        SearchForEscape();  
    }
    private void Update()
    {
        switch (state)
        {
            case EnemyState.running:
                MoveEnemy();
                RotateStatus();
                break;
            
            case EnemyState.caught:
                //isStopped = true;
                Debug.Log(gameObject.name + " has been caught");
                break;
        }
    }
    private List<Vector3> SetWaypoint(Vector3 stop)
    {
        MapManager mm = MapManager._Instance;
        Vector3Int start = mm.GetGridPos(transform.position);
        Vector3Int end = mm.GetGridPos(stop);
        List<Vector3> escapeNoodles = NAVI.FindPathVectors(start.x, start.y, end.x, end.y);
        return escapeNoodles;
    }

    private void SearchForEscape()
    {
        MapManager mm = MapManager._Instance;
        Tilemap tilemap = mm.map;
        BoundsInt bounds = mm.bounds;
        //float nearestDistance = 0;

 
        int[] yBounds = { bounds.max.y - 1, bounds.min.y };
        int[] xBounds = { bounds.max.x - 1, bounds.min.x };

        Vector3Int gridPos = mm.GetGridPos(transform.position);
        int xTarg = (Mathf.Abs(gridPos.x - xBounds[0]) > Mathf.Abs(gridPos.x - xBounds[1])) 
            ? xBounds[1] : xBounds[0];
        int yTarg = (Mathf.Abs(gridPos.y - yBounds[0]) > Mathf.Abs(gridPos.y - yBounds[1])) 
            ? yBounds[1] : yBounds[0];
        Vector3Int tileLocation = (Mathf.Abs(gridPos.x - xTarg) > Mathf.Abs(gridPos.y - yTarg)) 
            ? new Vector3Int(Random.Range(bounds.min.x, bounds.max.x), yTarg, 0) 
            : new Vector3Int(xTarg, Random.Range(bounds.min.y, bounds.max.y), 0);
        if (tilemap.HasTile(tileLocation))
        {
            closestTile = tilemap.GetCellCenterWorld(tileLocation);
            targDir = new Vector2(closestTile.x, closestTile.y) - new Vector2(transform.position.x, transform.position.y);
        }

        //for (int y = bounds.min.y, rangeY = bounds.max.y; y < rangeY; y++)
        //{
        //    for (int x = bounds.min.x, rangeX = bounds.max.x; x < rangeX; x++)
        //    {
        //        // skip if not a permiter tile
        //        if (!xBounds.Contains(x) && !yBounds.Contains(y)) continue;

        //        Vector3Int tileLocation = new Vector3Int(x, y, 0);
        //        if (tilemap.HasTile(tileLocation))
        //        {
        //            Vector3 cellWorldPos = tilemap.GetCellCenterWorld(tileLocation);
        //            Vector2 dir2 = new Vector2(cellWorldPos.x, cellWorldPos.y) - new Vector2(transform.position.x, transform.position.y);
        //            float distance2 = dir2.magnitude;
        //            if (distance2 < nearestDistance || nearestDistance == 0)
        //            {
        //                //Debug.Log("found closer tile: " + tileLocation + "\n" + cellWorldPos + " " + distance2);
        //                closestTile = cellWorldPos;
        //                nearestDistance = distance2;
        //                targDir = dir2;
        //            }
        //        }
        //    }
        //}
        Debug.Log(gameObject.name + " Nearest Escape: " + closestTile);
        escapeNoodles = SetWaypoint(closestTile);
    }

    private void MoveEnemy()
    {
        if (escapeNoodles != null)
        {
            Vector3 targ = escapeNoodles[pathIndex];
            if (Vector3.Distance(transform.position, targ) > 1f)
            {
                Vector3 moveDir = (targ - transform.position).normalized;
                // face direction
                transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;
            }
            else
            {
                pathIndex++;
                if (pathIndex >= escapeNoodles.Count)
                {
                    Debug.Log(gameObject.name + " has escaped out to sea");
                    escapeNoodles.Clear();
                    NAVI.ClearPath();
                    //gm.RemoveEnemy(gameObject);
                    Destroy(gameObject);
                    // lose points
                }
            }
        }
        else
        {
            // stop moving
            Debug.Log(gameObject.name + " has nowhere left to go");
        }
    }


    private void RotateStatus()
    {
        statusTime -= Time.deltaTime;
        if (statusTime <= 0)
        {
            statusIndex = (statusIndex + 1) % statuses.Length;
            StatusSwitchSprite();
        }
    }

    private void StatusSwitchSprite()
    {
        statusTime = statuses[statusIndex];
        anim.Play(statusAnims[statusIndex]);
        switch (statusIndex)
        {
            case 0:
                sr.enabled = false;
                Debug.Log("Hiding");
                break;
            case 1:
                sr.enabled = true;
                //icr.SetDirection(movement);
                Debug.Log("Bubbling");
                break;
            case 2:
                sr.enabled = true;
                //icr.SetDirection(movement, true);
                Debug.Log("Showing");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lasso"))
        {
            // check if lasso around stopping speed
            state = EnemyState.caught;
            anim.Play("Caught");
        }
    }
}
