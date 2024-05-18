using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarcoBehavior : MonoBehaviour
{
    [Header("Movement")]
    public enum EnemyState
    {
        running,
        caught
    }

    [SerializeField] private EnemyState state;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform escapeListParent;
    private Transform closestEscape;
    private Vector2 targDir;

    [Header("Sprite")]
    [SerializeField] private float timeHidden = 7f, timeShown = 7f, bubbleTime = 3f;
    [SerializeField] private float[] statuses = [timeHidden, bubbleTime, timeShown];
    private string[] statusAnims = ["Hide", "Bubble", "Show"];
    private Animator anim;

    public float statusTime;
    public int statusIndex;

    private void Start()
    {
        anim = GetComponent<Animator>();
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
                break;
        }
    }

    private void FindClosestEscape()
    {
        float nearestDistance;
        foreach (Transform child in escapeListParent)
        {
            Vector2 dir2 = child.position - transform.position;
            float distance2 = dir2.magnitude;
            if (distance2 < nearestDistance or nearestDistance == 0)
            {
                closestEscape = child;
                nearestDistance = distance2;
                targDir = dir2;
            }
        }
    }

    private void MoveEnemy()
    {
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

    private IEnumerator HideShowNarc()
    {
        yield return new WaitForSeconds(1f);
    }
}
