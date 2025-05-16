using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Info")]
    public string playerName = "Player1";
    [Header("Game state")]
    public string selectedGame = "";
    public List<Modifier> activeModifers = new();

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

    public void ResetModifiers()
    {
        activeModifers.Clear();
    }

    public float GetTotalScoreMultiplier()
    {
        float multiplier = 1f;
        foreach (var mod in activeModifers)
        {
            multiplier *= mod.scoreMultiplier;
        }
        return multiplier;
    }

    
}
