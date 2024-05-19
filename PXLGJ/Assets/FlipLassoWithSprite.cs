using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipLassoWithSprite : MonoBehaviour
{
    private void Update()
    {
        transform.localScale = (transform.parent.GetComponent<SpriteRenderer>().flipX) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }
}
