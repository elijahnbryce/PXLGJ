using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoPC : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float mSpd = 1f;
    private IsoCRenderer icr;
    private float hIn, vIn;
    private Vector2 dirIn, currPos, movement, newPos;
    private Rigidbody2D rb;

    [Header("Lasso")]
    [SerializeField] private Transform lassoPrefab;
    [SerializeField] private float  cooldown = 3f, projLife = 7f;
    private float throwStrength;
    private bool yeehaw = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        icr = GetComponentInChildren<IsoCRenderer>();
    }

    private void FixedUpdate()
    {
        currPos = rb.position;
        hIn = Input.GetAxisRaw("Horizontal");
        vIn = Input.GetAxisRaw("Vertical");
        dirIn = new Vector2(hIn - vIn, vIn + hIn); // + 45 degrees counter-clockwise
        dirIn = Vector2.ClampMagnitude(dirIn, 1);
        movement = dirIn * mSpd;
        newPos = currPos + movement * Time.fixedDeltaTime;
        icr.SetDirection(movement);
        rb.MovePosition(newPos);
    }

    private void Update()
    {
        if (!yeehaw)
        {
            if (Input.GetKey(KeyCode.Space))
            {

            }
            else StartCoroutine("ShootOnDelay()");
        }
    }

    private void Shoot()
    {
        Transform lasso = Instantiate(lassoPrefab, transform.parent.position, Quaternion.identity); // change Quaternion to dirIn
        lasso.GetComponent<Rigidbody2D>().AddForce(dirIn);
    }

    private IEnumerator ShootOnDelay()
    {
        yeehaw = true;
        Shoot();
        yield return new WaitForSeconds(cooldown);
        yeehaw = false;
    }
}
