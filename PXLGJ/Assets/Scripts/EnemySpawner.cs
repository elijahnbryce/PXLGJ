using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static GameManager gm;

    [Header("Time")]
    [SerializeField] private float spawnTime = 13f, shortestInterval = 3f;
    [SerializeField] private bool spawning = false;
    [SerializeField] private int spawnLimit = 10;

    [Header("Area")]
    [SerializeField] private GameObject narcoP, enemyHolder;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnrange = 47f;


    private void Start()
    {
        gm = GameManager._Instance;
    }

    private void Update()
    {
        if (!spawning) StartCoroutine(SpawnOnDelay());
    }

    private void Spawn()
    {
        if (gm.GetEnemyCount() <= spawnLimit)
        {
            float randomX = Random.Range(-spawnrange, spawnrange);
            float randomY = Random.Range(-spawnrange, spawnrange);
            Vector3 thisSpawn = new Vector3(spawnPoint.transform.position.x + randomX, spawnPoint.transform.position.y + randomY, 0);
            
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
