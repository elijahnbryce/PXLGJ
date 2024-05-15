using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoBehavior : MonoBehaviour
{
    private static GameManager gm;
    private Animator anim;
    private Rigidbody2D rb;
    
    [SerializeField] private float decelearation = 0.95f, maxStopSpeed = 1f;

    private void Start()
    {
        gm = GameManager._Instance;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // decel unless barely moving, then set to 0
        rb.velocity = (Mathf.Abs(rb.velocity.magnitude) > 0.1) ? rb.velocity * decelearation * Time.deltaTime : Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (rb.velocity.magnitude < maxStopSpeed)
            {
                CatchSub(collision.gameObject);
            }
        }
    }

    private static void CatchSub(GameObject narc)
    {
        // play catching animation
        Debug.Log("Catching Narco");
        // trigger sub caught
        gm.RemoveEnemy(narc); // change sub to call this after sub catch animation
    }
}
