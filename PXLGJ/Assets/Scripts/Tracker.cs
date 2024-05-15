using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    private static GameManager gm = GameManager._Instance;

    [SerializeField] GameObject waypoint;

    private Transform closest;
    private float nearestDistance;
    private Vector2 targDir;

    private void Update()
    {
        FindClosest();
        waypoint.transform.rotation = Quaternion.LookRotation(new Vector3(targDir.x, targDir.y, 0), Vector3.up);
    }

    private void FindClosest()
    {
        List<GameObject> list = gm.enemies;
        if (list.Count > 0)
        {
            ShowWaypoint();
            foreach (GameObject enemy in list)
            {
                Vector2 dir2 = enemy.transform.position - transform.position;
                float distance2 = dir2.magnitude;
                if (distance2 < nearestDistance)
                {
                    closest = enemy.transform;
                    nearestDistance = distance2;
                    targDir = dir2;
                }
            }
        }
        else ClearClosest();
    }

    public void ClearClosest()
    {
        HideWaypoint();
        closest = null;
        nearestDistance = 0;
        targDir = Vector2.zero;
    }

    public void HideWaypoint()
    {
        waypoint.SetActive(false);
    }

    public void ShowWaypoint()
    {
        waypoint.SetActive(true);
    }
}
