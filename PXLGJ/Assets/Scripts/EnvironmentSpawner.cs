using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [SerializeField] private int height, width, objSize = 3;
    [SerializeField] private Transform spawningParent;
    [SerializeField] private float spawnChance = 0.13f, spawnRange = 27f;
    [SerializeField] private List<GameObject> obstacles;
    [SerializeField] private GameObject objParent;
    // [SerializeField] private GameObject listObj;

    private void Start()
    {
        //TileSpawning();
        AreaSpawning();
    }

    private void TileSpawning()
    {
        for (int i = height / objSize; i > 0; i--)
        {
            for (int j = 0; j < width / objSize; j++)
            {
                int x = j * objSize;
                int y = i * objSize;
                /* For Isometric Tiles */
                    x = (x - y) / 2;
                    y - (y - x) / 4; 
                if (Random.Range(0, 1) < spawnChance) 
                {
                    int index = Random.Range(0, obstacles.Count);
                    Transform spwntrans = new Vector3(x , y, 1);
                    Instantiate(obstacles[index], spwntrans, Quaternion.Identity);
                }
            }
        }
    }

    private void AreaSpawning()
    {
        height = spawningParent.scale.y / 2;
        width = spawningParent.scale.x / 2;

        for (int i = 0; i < height / objSize; i++)
        {
            for (int j = 0; j < width / objSize; j++)
            {
                int x = j * objSize;
                int y = i * objSize;
                for (int k = 0; k < 2; k++)
                {
                    if (Random.Range(0, 1) < spawnChance) 
                    {
                        int index = Random.Range(0, obstacles.Count);
                        Transform spwntrans = new Vector3(spawningParent.position.x - (x * ((k * 2) - 1)) , spawningParent.position.y - (y * ((k * 2) - 1)), 1);
                        Instantiate(obstacles[index], spwntrans, Quaternion.Identity);
                    }
                }
            }
        }
    }
}
