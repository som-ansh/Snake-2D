using TMPro;
using Unity.AppUI.UI;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText, livesText;
    public Button mainMenuButton;
    private Snake snakeScript;

    private void Start()
    {
        snakeScript = GameObject.Find("Snake").GetComponent<Snake>();
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + snakeScript.score;
    }
}
