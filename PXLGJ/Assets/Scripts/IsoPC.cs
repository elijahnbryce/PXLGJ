using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoPC : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float mSpd = 1f;
    private IsoCRenderer icr;
    private float hIn, vIn;
    private Vector2 dirIn, lookDir, currPos, movement, newPos;
    private Rigidbody2D rb;

    [Header("Lasso")]
    [SerializeField] private Transform lassoPrefab;
    [SerializeField] private Transform lassoDecal;
    [SerializeField] private float maxPower = 47f, maxHold = 3f, cooldown = 3f, projLife = 3f;
    private float throwStrength, shootStartTime, shootDownDuration;
    private bool yeehaw = false;
    private PowerBar pbnj;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        icr = GetComponentInChildren<IsoCRenderer>();
    }

    private void Start()
    {
        pbnj = GetComponent<PowerBar>();
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
        if (dirIn.magnitude > 0.1f) lookDir = dirIn;
    }

    private void Update()
    {
        if (!yeehaw) Hold2Shoot();
    }

    private void Hold2Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shootStartTime = Time.time;
            pbnj.ShowBar();
            lassoDecal.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(ShootOnDelay());
            pbnj.HideBar();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            shootDownDuration = Mathf.Min(Time.time - shootStartTime, maxHold);
            throwStrength = CalcLaunchPower(shootDownDuration);
            pbnj.UpdateBar(shootDownDuration);
        }
    }

    private float CalcLaunchPower(float holdTime)
    {
        float holdTimeNormal = Mathf.Clamp01(holdTime / maxHold);
        return holdTimeNormal * maxPower;
    }

    private void Shoot()
    {  
        lassoDecal.gameObject.SetActive(false);
        Quaternion quaternion = Quaternion.LookRotation(Vector3.forward, lookDir);

        Transform lasso = Instantiate(lassoPrefab, lassoDecal.position, quaternion);
        lasso.GetComponent<Rigidbody2D>().velocity = lookDir * throwStrength;
        Destroy(lasso.gameObject, projLife);
    }

    private IEnumerator ShootOnDelay()
    {
        yeehaw = true;
        Shoot();
        yield return new WaitForSeconds(cooldown);
        shootStartTime = Time.time; // in case Space held down before player can shoot resulting in no GetKeyDown call and superFast projectile
        yeehaw = false;
    }
}
