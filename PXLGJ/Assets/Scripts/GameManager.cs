using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance;
    [SerializeField] private EnemySpawner enemySpawn;
    public List<GameObject> enemies;

    // private EnvironmentSpawner envSpawn;

    private int score = 0, lives = 3;

    private void Awake()
    {
        _Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //enemySpawn = GetComponent<EnemySpawner>();
    }

    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public void LoseGame()
    {

    }

    public void WinGame()
    {

    }

    public void PauseGame()
    {
        
    }

    public void Restart()
    {

    }

    public void UpdateScore()
    {

    }

    public void UpdateLives()
    {

    }}
