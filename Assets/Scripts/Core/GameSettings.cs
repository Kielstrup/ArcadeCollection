using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    public bool isVsAI = false; 

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
