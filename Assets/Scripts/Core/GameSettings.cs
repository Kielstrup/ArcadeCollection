using UnityEngine;

/// <summary>
/// Singleton class to store and persist game settings across scenes.
/// </summary>
public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    /// <summary>
    /// Flag indicating if the AI mode is enabled.
    /// </summary>
    public bool isVsAI = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Remove duplicates
        }
    }
}
