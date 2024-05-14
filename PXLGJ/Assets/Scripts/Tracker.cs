using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    // private GameManger gm = GameManager._Instance;

    [SerializeField] private Transform waypoint;

    private Transform closest;
    private float nearestDistance;
    private Vector2 targDir;

    private void Update()
    {
        FindClosest();
        // waypoint.rotation = targDir;
    }

    private void FindClosest()
    {
        // foreach transform enemy in gm.GetEnemiesList()
            // Vector2 dir2 = enemy.position - transform.position;
            // float distance2 = dir2.magnitude;
            // if (distance2 < nearestDistance)
                // closest = enemy;
                // nearestDistance = distance2;
                // targDir = dir2;
    }
}
