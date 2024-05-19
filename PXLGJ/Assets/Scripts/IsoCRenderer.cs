using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCRenderer : MonoBehaviour
{
    public static readonly string[] sDirs = {"Static N", "Static NW", "Static W", "Static SW", "Static S", "Static SE", "Static E", "Static NE" };
    public static readonly string[] rDirs = {"Run N", "Run NW", "Run W", "Run SW", "Run S", "Run SE", "Run E", "Run NE" };

    private Animator anim;
    private int lastDir; //holds player direction while not moving

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetDirection(Vector2 dir)
    {
        // default rDirs
        string[] dirsArray = null;

        //measure magnitude input
        if (dir.magnitude < 0.1f)
        {
            // not moving use static
            dirsArray = sDirs;
        }
        else
        {
            dirsArray = rDirs;
            lastDir = DirectionToIndex(dir, 8);
        }

        anim.Play(dirsArray[lastDir]);
    }

    // convert Vector2 direction to an array index in slices around a circle
    // counter clockwise steps
    public static int DirectionToIndex(Vector2 dir, int sliceCount)
    {
        Vector2 normalD = dir.normalized;
        // slice the pie
        float step = 360f / sliceCount;
        // offset in half-steps to count counterclockwise from North (UP) slice
        float halfstep = step / 2;
        // angle from -180 to 180 of the direction vector relative to Up Vector
        // returns angle between dir and North
        float angle = Vector2.SignedAngle(Vector2.up, normalD);
        // add halfstep offset
        angle += halfstep;
        // negative angles +360 to wrap around
        // this means 0-180 = west and 180-360 = east
        if (angle < 0) angle += 360;
        // steps to angle
        // (index in Dirs array)
        float stepCount = angle / step;
        // round that bihh
        return Mathf.FloorToInt(stepCount);
    }
}
