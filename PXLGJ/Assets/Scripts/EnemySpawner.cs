using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    private static GameManager gm;
    private static MapManager mm;

    [Header("Time")]
    [SerializeField] private float spawnTime = 13f, shortestInterval = 3f;
    [SerializeField] private bool spawning = false;
    [SerializeField] private int spawnLimit = 10;

    [Header("Area")]
    [SerializeField] private GameObject narcoP, enemyHolder;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int spawnrange = 7;

    [Header("Tilemap")]
    private Tilemap tilemap;
    private BoundsInt bounds;


    private void Start()
    {
        gm = GameManager._Instance;
        mm = MapManager._Instance;
        tilemap = MapManager._Instance.map;
        bounds = MapManager._Instance.bounds;
    }

    private void Update()
    {
        if (!spawning) StartCoroutine(SpawnOnDelay());
    }

    private void Spawn()
    {
        if (gm.GetEnemyCount() <= spawnLimit)
        {
            int randomX = Random.Range(bounds.min.x + spawnrange, bounds.max.x - spawnrange);
            int randomY = Random.Range(bounds.min.y + spawnrange, bounds.max.y - spawnrange);
            Vector3 thisSpawn = tilemap.GetCellCenterWorld(new Vector3Int(randomX, randomY));
            //Vector3 thisSpawn = new Vector3(spawnPoint.transform.position.x + randomX, spawnPoint.transform.position.y + randomY, 0);
            
            GameObject newNarc = Instantiate(narcoP, thisSpawn, spawnPoint.rotation);
            newNarc.transform.parent = enemyHolder.transform;
            gm.AddEnemy(newNarc);
        }
    }

    private IEnumerator SpawnOnDelay()
    {
        spawning = true;
        Spawn();
        yield return new WaitForSeconds(spawnTime);
        spawning = false;
    }
}
