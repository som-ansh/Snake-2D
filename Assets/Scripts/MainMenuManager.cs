using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public GameState currentState = GameState.Initial;

    private void Awake()
    {
        
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void PlayButton()
    {
        if (currentState == GameState.Initial)
        {
            currentState = GameState.Playing;
            SceneManager.LoadScene(1);
        }

    }

}
