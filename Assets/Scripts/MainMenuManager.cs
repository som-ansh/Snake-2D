using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    
    public void PlayButton()
    {
        
        if (MainManager.instance.currentState == GameState.Initial)
        {
            MainManager.instance.currentState = GameState.Playing;
            SceneManager.LoadScene(1);
        }

    }

}
