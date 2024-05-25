using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.U2D.IK;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance;

    [Header("Level")]
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject levelManager;
    [SerializeField] private Tilemap tilemapBorder, edgesMap;
    public List<GameObject> enemies;
    private EnemySpawner enemySpawn;
    private EnvironmentSpawner envSpawn;

    [Header("Canvas")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winCan, loseCan, ovrCan;

    [SerializeField] private TextMeshProUGUI loseScore, winScore, goalText, livesText, hsText, timerText;
    [SerializeField] private TMP_InputField hsInput;

    [Header("Game Status")]
    public int lives = 3;
    public int score;
    //public Timer ts;
    //public float totalTime;
    private bool gamePaused, gameActive, noPausing;

    private void Awake()
    {
        if (_Instance == null) 
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        enemySpawn = levelManager.GetComponent<EnemySpawner>();
        envSpawn = levelManager.GetComponent<EnvironmentSpawner>();
        SetLevel();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) PauseGame();
    }

    public void SetLevel()
    {
        Time.timeScale = 1;
        score = 0;

        cam = GameObject.Find("Main Camera");
        //ts = GameObject.Find("Timer").GetComponent<Timer>();
        //ts.SetTB(timerText);

        EverythingFalse();
        UpdateScore(0);
        UpdateLives(0);
    }

    private void EverythingFalse()
    {
        gameActive = true;
        gamePaused = false;
        noPausing = false;

        ovrCan.SetActive(false);
        hsInput.gameObject.SetActive(false);
        winCan.SetActive(false);
        loseCan.SetActive(false);
        pauseMenu.SetActive(false);

        winCan.GetComponent<Canvas>().worldCamera = cam.GetComponent<Camera>();
        loseCan.GetComponent<Canvas>().worldCamera = cam.GetComponent<Camera>();
        pauseMenu.GetComponent<Canvas>().worldCamera = cam.GetComponent<Camera>();

        //ts.StartTime();
    }

    public void Restart()
    {
        if (!gamePaused) return;
        if (gameActive) LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
        {
            LoadScene();
            KillSelf();
        }
    }

    public void LoadScene(int sceneNum = 0)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void EndLevel(bool result, int lvlScore)
    {
        Time.timeScale = 0;
        gameActive = false;
        gamePaused = true;
        noPausing = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (result) WinGame(lvlScore);
        else LoseGame(lvlScore);
        CheckHS();
    }

    private void CheckHS()
    {
        cam.GetComponent<AudioSource>().Stop();
        ovrCan.SetActive(true);
        int highScore = PlayerPrefs.GetInt("HIGHSCORE");
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HIGHSCORE", score);
            hsText.text = "New High Score!";
            hsInput.gameObject.SetActive(true);
        }
        else
        {
            hsText.text = "*+ " + PlayerPrefs.GetString("HIGHSCORENAME") + ": " + highScore + " +*";
        }
    }

    public void NewHighScore()
    {
        string hsName = hsInput.text;
        PlayerPrefs.SetString("HIGHSCORENAME", hsName);
        hsInput.gameObject.SetActive(false);
    }

    private void LoseGame(int finalScore)
    {
        loseCan.SetActive(true);
        loseScore.text = finalScore.ToString();
    }

    private void WinGame(int finalScore)
    {
        winCan.SetActive(true);
        winScore.text = finalScore.ToString();
    }

    public void PauseGame()
    {
        if (noPausing) return;
        if (gamePaused)
        {
            pauseMenu.SetActive(false);
            gamePaused = false;
            gameActive = true;
            Time.timeScale = 1;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0;
            gameActive = false;
            gamePaused = true;
            pauseMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void QuitApp()
    {
        LoadScene(0);
        KillSelf();
        Application.Quit();
    }

    public void UpdateScore(int change = 1)
    {
        score += change;
        goalText.text = "Score: " + score.ToString();
    }

    public void UpdateLives(int change = -1)
    {
        lives += change;
        livesText.text = "Lives: " + lives.ToString();

        if (lives <= 0)
        {
            lives = 0;
            EndLevel(false, score);
        }
    }
    public void incLives(int val)
    {
        UpdateLives(1);
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    public Tilemap GetTilemap()
    {
        return tilemapBorder;
    }

    public void KillSelf()
    {
        Destroy(gameObject);
    }
}
