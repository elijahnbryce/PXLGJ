using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NarcoBehavior : MonoBehaviour
{
    public enum EnemyState
    {
        running,
        caught
    }

    [Header("Movement")]
    [SerializeField] private EnemyState state;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform escapeListParent;
    private Transform closestEscape;
    private Vector2 targDir;
    private NavMeshAgent NAVI;


    [Header("Sprite")]
    [SerializeField] private static float timeHidden = 7f, timeShown = 7f, bubbleTime = 3f;
    [SerializeField] private float[] statuses = { timeHidden, bubbleTime, timeShown };
    private string[] statusAnims = { "Hide", "Bubble", "Show" };
    private Animator anim;

    public float statusTime;
    public int statusIndex;

    private void Start()
    {
        anim = GetComponent<Animator>();
        NAVI = GetComponent<NavMeshAgent>();
        FindClosestEscape();

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
                NAVI.isStopped = true;
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
