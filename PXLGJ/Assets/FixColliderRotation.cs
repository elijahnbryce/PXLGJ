using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixColliderRotation : MonoBehaviour
{
    private SpriteRenderer sr;
    private GameObject colliderObj;
    private float zRot, mult;
    [SerializeField] private bool reverse;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        colliderObj = GetComponentInChildren<CapsuleCollider2D>().gameObject;
    }

    private void Update()
    {
        mult = (reverse) ? -1 : 1;
        zRot = (sr.flipX) ? -30f : 30f;
        zRot *= mult;
        colliderObj.transform.rotation = Quaternion.Euler(0, 0, 60 + zRot);
    }
}
