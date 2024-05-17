using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LassoBehavior : MonoBehaviour
{
    private GameManager gm;
    private Animator anim;
    private Rigidbody2D rb;
    
    [SerializeField] private float decelearation = 0.95f, maxStopSpeed = 1f;
    public float speed;

    private void Start()
    {
        gm = GameManager._Instance;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {   
        speed = rb.velocity.magnitude;
        // decel unless barely moving, then set to 0
        rb.velocity = (Mathf.Abs(rb.velocity.magnitude) > 0.1) ? rb.velocity * decelearation : Vector2.zero;
        anim.SetBool("Stopped", Mathf.Abs(rb.velocity.magnitude) > 0.1);
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

    private void CatchSub(GameObject narc)
    {
        anim.SetTrigger("Catch");
        Debug.Log("Catching Narco");
        // trigger sub caught
        gm.RemoveEnemy(narc); // change sub to call this after sub catch animation
    }
}
