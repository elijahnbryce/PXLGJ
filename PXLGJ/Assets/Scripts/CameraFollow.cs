using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float zOff = -10f, camDelay = 0.2f;
    [SerializeField] private Transform player;

    private Vector3 camVelocity = Vector3.zero;

    private void Update()
    {
        Vector3 camPos = new Vector3(player.transform.position.x, player.transform.position.y, zOff);
        transform.position = Vector3.SmoothDamp(transform.position, camPos, ref camVelocity, camDelay);
    }
}
