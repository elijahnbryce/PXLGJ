using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
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

    private GameManager gm = GameManager._Instance;

    [Header("Movement")]
    [SerializeField] private SpriteStatus status;
    [SerializeField] private EnemyState state;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform escapeListParent;
    private Transform closestEscape;
    private Vector3 closestTile;
    private Vector2 targDir;
    private NavMeshAgent NAVI;
    private IsoCRenderer icr;


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
        NAVI = GetComponent<NavMeshAgent>();
        icr = GetComponentInChildren<IsoCRenderer>();
        sr = GetComponent<SpriteRenderer>();

        //FindClosestEscape();
        //FindClosesTile();  
    }
    private void Update()
    {
        switch (state)
        {
            case EnemyState.running:
                //MoveEnemy();
                RotateStatus();
                break;
            
            case EnemyState.caught:
                //NAVI.isStopped = true;
                break;
        }
    }
    private void SetWaypoint(Vector3 pos)
    {
        NAVI.SetDestination(pos);
    }

    private void FindClosestEscape()
    {
        float nearestDistance = 0;
        foreach (Transform child in escapeListParent)
        {
            Vector2 dir2 = child.position - transform.position;
            float distance2 = dir2.magnitude;
            if (distance2 < nearestDistance || nearestDistance == 0)
            {
                closestEscape = child;
                nearestDistance = distance2;
                targDir = dir2;
            }
        }

        SetWaypoint(closestEscape.position);

        /*
        GetComponent<Tilemap>().CompressBounds();
        GetComponent<Tilemap>().size;
         * tile size?
        bottom left => Vector3 TileOrigin = Camera.main.WorldToScreenPoint(tilemap.origin)
        */
    }

    private void FindClosesTile()
    {
        Tilemap tilemap = gm.GetTilemap();
        BoundsInt bounds = tilemap.cellBounds;
        float nearestDistance = 0;

        // looping through all tiles
        for (int y = bounds.min.y; y < bounds.max.y; y++)
        {
            for (int x = bounds.min.x; x < bounds.max.x; x++)
            {
                Vector3Int tileLocation = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(tileLocation))
                {
                    Vector3 cellWorldPos = tilemap.GetCellCenterWorld(tileLocation);
                    Vector2 dir2 = new Vector2(cellWorldPos.x, cellWorldPos.y) - new Vector2(transform.position.x, transform.position.y);
                    float distance2 = dir2.magnitude;
                    if (distance2 < nearestDistance || nearestDistance == 0)
                    {
                        closestTile = cellWorldPos;
                        nearestDistance = distance2;
                        targDir = dir2;
                    }
                }
            }
        }
        SetWaypoint(closestTile);
    }

    private void MoveEnemy()
    {
        NAVI.isStopped = false;
        // Navmesh probablby
        // move twrds closestEscape
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
