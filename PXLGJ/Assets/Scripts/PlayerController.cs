using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float nSpeed = 7f;
    private float xdir, ydir;
    private Vector2 lookdir;
    private Rigidbody2D rb;

    [Header("Sprites")]
    private SpriteRenderer psprite;
    public List<Sprite> currDir;
    public List<Sprite> nS, nxS, xS, sxS, sS;
    private float frameRate = 6, idleTime;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // get input direction
        xdir = Input.GetAxisRaw("Horizontal");
        ydir = Input.GetAxisRaw("Vertical");
        lookdir = new Vector2(xdir, ydir).normalized;

        // set movement direction
        rb.velocity = lookdir * nSpeed * Time.deltaTime;

        // (only when input)
        if (lookdir != Vector2.zero)
        {
            HandleSpriteFlip();
            SetSprite();
        }
    }

    private void HandleSpriteFlip()
    {
        if (!psprite.flipX && lookdir.x < 0) psprite.flipX = true;
        else if (psprite.flipX && lookdir.x > 0) psprite.flipX = false;
    }

    private void SetSprite()
    {
        currDir = GetSpriteDir();
        if (currDir != null)
        {
            float playTime = Time.deltaTime - idleTime;
            int totalFrames = (int)(playTime * frameRate);
            int frame = totalFrames % currDir.Count;
            psprite.sprite = currDir[frame];
        }
        else idleTime = Time.deltaTime;
    }

    private List<Sprite> GetSpriteDir()
    {
        List<Sprite> selectedSprites = null;
        bool xMovement = (Mathf.Abs(lookdir.x) > 0);

        if (lookdir.y > 0)        // north
            selectedSprites = xMovement ? nxS : nS;
        else if (lookdir.y < 0)   // south
            selectedSprites = xMovement ? sxS : sS;
        else                      // null
            selectedSprites = xMovement ? xS : null;

        return selectedSprites;
    }
}
