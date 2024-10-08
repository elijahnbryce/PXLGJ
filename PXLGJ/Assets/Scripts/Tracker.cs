using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    private static GameManager gm = GameManager._Instance;

    [SerializeField] private GameObject waypoint, testEnemy;
    [SerializeField] private float blinkTime = 2f, diffOnOff = 2f;

    private Transform closest;
    private float nearestDistance;
    private Vector2 targDir;
    private bool blinking = false;

    private void Update()
    {
        //FindClosest();
        //PointTracker();
        if (testEnemy != null)
        {
            targDir = testEnemy.transform.position - transform.position;
            if (!blinking) StartCoroutine(BlinkWaypoint());
        }
    }

    private IEnumerator BlinkWaypoint()
    {
        blinking = true;
        yield return new WaitForSeconds(blinkTime);
        PointTracker();
        yield return new WaitForSeconds(blinkTime / diffOnOff);
        waypoint.SetActive(false);
        blinking = false;

    }
    private void PointTracker()
    {
        waypoint.SetActive(true);
        waypoint.transform.rotation = Quaternion.LookRotation(Vector3.forward, targDir.normalized);
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
