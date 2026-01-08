using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;
    public GameState currentState;
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
    void Start()
    {
        currentState = GameState.Initial;
    }

}
