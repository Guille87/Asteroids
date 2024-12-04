using System.IO;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const int LIVES = 3;
    const int EXTRA_LIFE = 1500;
    const int SCORE_ENEMY = 50;
    const int SCORE_ASTEROID_BIG = 10;
    const int SCORE_ASTEROID_SMALL = 25;
    const string DATA_FILE = "data.json";

    static GameManager instance;

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI tmpScore;
    [SerializeField] TextMeshProUGUI tmpHScore;
    [SerializeField] TextMeshProUGUI tmpMessage;

    [Header("Player Lives")]
    [SerializeField] Image[] imgLives;

    [Header("Sound Effects")]
    [SerializeField] AudioClip sfxExtra;
    [SerializeField] AudioClip sfxGameOver;

    int score;
    int lives = LIVES;

    bool extra;
    bool gameOver;
    bool paused;
    
    GameData gameData;

    public bool isGameOver() { return gameOver; }
    public bool isGamePaused() { return paused; }

    public static GameManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        gameData = LoadData();
    }

    GameData LoadData()
    {
        if (File.Exists(DATA_FILE))
        {
            string fileText = File.ReadAllText(DATA_FILE);
            return JsonUtility.FromJson<GameData>(fileText);
        }
        else
            return new GameData();
    }

    void SaveData()
    {
        // creamos la representación JSON de gamedate
        string json = JsonUtility.ToJson(gameData);

        // volcar sobre el archivo
        File.WriteAllText(DATA_FILE, json);
    }

    public void AddScore(string tag)
    {
        int pts = 0;

        switch (tag)
        {
            case "Enemy":
                pts = SCORE_ENEMY;
                break;
            case "AsteroidBig":
                pts = SCORE_ASTEROID_BIG;
                break;
            case "AsteroidSmall":
                pts = SCORE_ASTEROID_SMALL;
                break;
        }

        score += pts;

        // check extra life
        if (!extra && score >= EXTRA_LIFE)
            ExtraLife();
        
        // actualiza hscore
        if (score > gameData.hscore )
            gameData.hscore = score;
    }

    void ExtraLife()
    {
        extra = true;
        lives++;
        AudioSource.PlayClipAtPoint(sfxExtra, Camera.main.transform.position, 0.2f);
    }

    public void LoseLife()
    {
        lives--;

        if (lives == 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOver = true;
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(sfxGameOver, Camera.main.transform.position, 0.2f);
        tmpMessage.text = "GAME OVER\nPRESS <RET> TO RESTART";
        SaveData();
    }

    private void OnGUI()
    {
        // activar los iconos de las vidas
        for (int i = 0; i < imgLives.Length; i++)
        {
            imgLives[i].enabled = i < lives-1;
        }

        // mostrar la puntuación del jugador
        tmpScore.text = string.Format("{0,4:D4}", score);

        // mostrar la puntuación máxima
        tmpHScore.text = string.Format("{0,4:D4}", gameData.hscore);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        else if (!gameOver) {
            if(Input.GetKeyDown(KeyCode.P))
            {
                if (paused)
                    ResumeGame();
                else
                    PauseGame();
            }
            else if (Input.GetKeyDown(KeyCode.F1))
                Time.timeScale /= 1.25f;
            else if (Input.GetKeyDown(KeyCode.F2))
                Time.timeScale *= 1.25f;
            else if (Input.GetKeyDown(KeyCode.F3))
                Time.timeScale = 1;
        }
        else if (gameOver && Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(0);
    }

    void PauseGame()
    {
        paused = true;
        Camera.main.GetComponent<AudioSource>().Pause();
        tmpMessage.text = "GAME PAUSED\nPRESS <P> TO RESUME";
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        paused = false;
        Camera.main.GetComponent<AudioSource>().UnPause();
        tmpMessage.text = "";
        Time.timeScale = 1;
    }
}
