using UnityEngine;
using System.IO;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance;

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

    private string GetFilePath(string gameName)
    {
        return Path.Combine(Application.persistentDataPath, gameName + "_highscores.json");
    }

    public void SaveHighScores(string gameName, HighScoreData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetFilePath(gameName), json);
        Debug.Log($"High scores saved for {gameName} at {GetFilePath(gameName)}");
    }

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

    public void AddHighScore(string gameName, string playerName, int score)
    {
        HighScoreData data = LoadHighScores(gameName);
        data.entries.Add(new HighScoreEntry() { playerName = playerName, score = score });
        data.entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (data.entries.Count > 10)
        {
            data.entries.RemoveRange(10, data.entries.Count - 10);

        }

        SaveHighScores(gameName, data);
    }
}
