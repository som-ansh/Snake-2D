using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Playing,
    GameOver,
    GamePaused,
    Initial
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
   
    public TextMeshProUGUI scoreText, livesText;
    public Image gameOver;
    public Button mainMenuButton, pauseButton;
    public GameObject pausePanel;
    public Snake snakeScript;
    private AudioSource audioSource;
    public AudioClip gameOverAudio;
    
    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameOver.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
    }

    public void UpdateScore()
    {
        snakeScript.score++;
        scoreText.text = "Score: " + snakeScript.score;
        snakeScript.UpdateMoveRate();
    }

    public void UpdateLives()
    {
        if (snakeScript.lives > 0)
        {
            snakeScript.lives--;
            livesText.text = "Lives: " + snakeScript.lives;
            StartCoroutine(snakeScript.BlinkLives());
        }
        else
        {
            GameOver();
        }
    }
    
    public void GameOver()
    {
        MainManager.instance.currentState = GameState.GameOver;
        audioSource.Pause();
        audioSource.PlayOneShot(gameOverAudio, 1.0f);
        audioSource.Play();
        gameOver.gameObject.SetActive(true);
        gameOver.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "GAME OVER !" + "\n" + "Your Score: " + snakeScript.score;
    }

    public void PauseGame()
    {
        if(MainManager.instance.currentState != GameState.GamePaused)
        {
            MainManager.instance.currentState = GameState.GamePaused;
            Time.timeScale = 0.0f;
            pausePanel.SetActive(true);
        }

    }
    public void ResumeGame()
    {
        if (MainManager.instance.currentState == GameState.GamePaused)
        {
            MainManager.instance.currentState = GameState.Playing;
            Time.timeScale = 1.0f;
            pausePanel.SetActive(false);
        }
    }

    public void ReturnToMainMenu()
    {
        if(MainManager.instance.currentState == GameState.Playing || MainManager.instance.currentState == GameState.GameOver)
        {
            MainManager.instance.currentState = GameState.Initial;
            SceneManager.LoadScene(0);
        }
    }
}
