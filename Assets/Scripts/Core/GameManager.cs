using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// NOTE: This script was intended to manage player and game state across scenes,
/// including player info, selected game mode, and active gameplay modifiers.
/// It provides utility functions like calculating total score multipliers from modifiers.
///
/// It is functionally used by the Modifier Debug script, which is active in the project,
/// but both this script and the debug script are ultimately unnecessary in the final product.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance;

    [Header("Player Info")]
    /// <summary>
    /// The current player's name.
    /// </summary>
    public string playerName = "Player1";

    [Header("Game state")]
    /// <summary>
    /// The currently selected game mode or title.
    /// </summary>
    public string selectedGame = "";

    /// <summary>
    /// List of active gameplay modifiers that affect score or other gameplay aspects.
    /// </summary>
    public List<Modifier> activeModifers = new();

    private void Awake()
    {
        // Ensure only one instance of GameManager exists (Singleton pattern)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    /// <summary>
    /// Clears all active modifiers.
    /// Useful for resetting game state before starting a new game.
    /// </summary>
    public void ResetModifiers()
    {
        activeModifers.Clear();
    }

    /// <summary>
    /// Calculates the total score multiplier by multiplying
    /// all individual score multipliers from active modifiers.
    /// </summary>
    /// <returns>The combined score multiplier as a float.</returns>
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
