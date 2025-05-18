using UnityEngine;
using System.IO;

/// <summary>
/// Singleton manager responsible for loading, saving, and managing high scores.
/// </summary>
public class HighScoreManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the HighScoreManager.
    /// </summary>
    public static HighScoreManager Instance;

    /// <summary>
    /// Ensures only one instance of the manager exists and persists across scenes.
    /// </summary>
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

    /// <summary>
    /// Constructs the full file path for storing high scores of a specific game.
    /// </summary>
    /// <param name="gameName">The name of the game (used to differentiate files).</param>
    /// <returns>Full file path as a string.</returns>
    private string GetFilePath(string gameName)
    {
        return Path.Combine(Application.persistentDataPath, gameName + "_highscores.json");
    }

    /// <summary>
    /// Saves the given high score data to disk as JSON.
    /// </summary>
    /// <param name="gameName">Name of the game to save scores for.</param>
    /// <param name="data">The high score data to save.</param>
    public void SaveHighScores(string gameName, HighScoreData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetFilePath(gameName), json);
        Debug.Log($"High scores saved for {gameName} at {GetFilePath(gameName)}");
    }

    /// <summary>
    /// Loads high scores from disk. If no file exists, returns an empty data object.
    /// </summary>
    /// <param name="gameName">Name of the game to load scores for.</param>
    /// <returns>Loaded HighScoreData or new empty data if none exists.</returns>
    public HighScoreData LoadHighScores(string gameName)
    {
        string path = GetFilePath(gameName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<HighScoreData>(json);
        }
        else
        {
            Debug.Log($"No high score file found for {gameName}, creating new data");
            return new HighScoreData();
        }
    }

    /// <summary>
    /// Adds a new high score entry and maintains a sorted top-10 list.
    /// </summary>
    /// <param name="gameName">Name of the game.</param>
    /// <param name="playerName">Player's name or initials.</param>
    /// <param name="score">Score achieved by the player.</param>
    public void AddHighScore(string gameName, string playerName, int score)
    {
        HighScoreData data = LoadHighScores(gameName);
        data.entries.Add(new HighScoreEntry() { playerName = playerName, score = score });
        // Sort descending by score
        data.entries.Sort((a, b) => b.score.CompareTo(a.score));

        // Keep only top 10 scores
        if (data.entries.Count > 10)
        {
            data.entries.RemoveRange(10, data.entries.Count - 10);
        }

        SaveHighScores(gameName, data);
    }
}
