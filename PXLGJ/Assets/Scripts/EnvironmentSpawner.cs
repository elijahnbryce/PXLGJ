using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentSpawner : MonoBehaviour
{
    [SerializeField] private int objSize = 3;
    [SerializeField] private Transform spawningParent;
    [SerializeField] private float spawnChance = 0.13f, spawnRange = 27f;
    [SerializeField] private List<GameObject> obstacles;
    //[SerializeField] private GameObject objPref;
    [SerializeField] public Tilemap tilemap;
    // [SerializeField] private GameObject listObj;

    private MapManager MM;

    private void Start()
    {
        MM = MapManager._Instance;
        TileSpawning(); // block Left(-1,0) Down(-1,-1) Right(0,-1) : tile on (7,6) should block (6,6) (6,5) (7,5)
        //AreaSpawning();
    }
    
    private void TileSpawning()
    {
        BoundsInt bounds = tilemap.cellBounds;

        // looping through all tiles
        for (int j = bounds.min.y, j1 = bounds.max.y / objSize; j < j1; j++)
        {
            for (int k = bounds.min.x, k1 = bounds.max.x / objSize; k < k1; k++)
            {
                int x = k * objSize;
                int y = j * objSize;
                int z = 0;
                /* For Isometric Tiles 
                x = (x - y) / 2;
                y = (y - x) / 4;*/

                Vector3Int tileLocation = new Vector3Int(x, y, z);
                if (tilemap.HasTile(tileLocation))
                {
                    if (Random.Range(0f, 1f) < spawnChance * GameManager._Instance.difficultySlider)
                    {
                        var cellWorldPos = tilemap.GetCellCenterWorld(tileLocation);
                        Vector3 spwntrans = new Vector3(cellWorldPos.x, cellWorldPos.y, cellWorldPos.z + 1);

                        int index = Random.Range(0, obstacles.Count);
                        var obstacle = Instantiate(obstacles[index], spawningParent);
                        obstacle.transform.position = spwntrans;
                        obstacle.GetComponent<SpriteRenderer>().sortingOrder = tilemap.GetComponent<TilemapRenderer>().sortingOrder + 1;

                        MM.SetTileUnwalkable(spwntrans);
                    }
                }
            }
        }
        GetComponent<NavMeshOven>().GenerateLevel();
    }
/*
    private void AreaSpawning()
    {
        height = Mathf.FloorToInt(spawningParent.localScale.y / 2);
        width = Mathf.FloorToInt(spawningParent.localScale.x / 2);

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
                        Vector3 spwntrans = new Vector3(spawningParent.position.x - (x * ((k * 2) - 1)) , spawningParent.position.y - (y * ((k * 2) - 1)), 1);
                        Instantiate(obstacles[index], spwntrans, Quaternion.identity);
                    }
                }
            }
        }
    }*/
}
