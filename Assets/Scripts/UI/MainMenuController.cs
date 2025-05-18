using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject panelMain;
    public GameObject panelHighScores;
    public GameObject panelHighScoreList;
    public GameObject panelSettings;
    public GameObject panelSelectGame;
    public TextMeshProUGUI textGameTitle; // Assign in Inspector
    public Transform scoresContainer; // Assign in Inspector
    public GameObject scoreTextPrefab; // A prefab or just create Text objects dynamically (optional)
    public GameObject panelPongModeSelect;
    public GameObject panelStartTetris;






    public void ShowMain()
    {
        panelMain.SetActive(true);
        panelHighScores.SetActive(false);
        panelHighScoreList.SetActive(false);
        panelSettings.SetActive(false);
        panelSelectGame.SetActive(false);
        panelPongModeSelect.SetActive(false);
        panelStartTetris.SetActive(false);
    }

    public void ShowHighScores()
    {
        panelMain.SetActive(false);
        panelHighScores.SetActive(true);
        panelHighScoreList.SetActive(false);
        panelSettings.SetActive(false);
        panelSelectGame.SetActive(false);
        panelPongModeSelect.SetActive(false);
        panelStartTetris.SetActive(false);
    }

    public void ShowSettings()
    {
        panelMain.SetActive(false);
        panelHighScores.SetActive(false);
        panelHighScoreList.SetActive(false);
        panelSettings.SetActive(true);
        panelSelectGame.SetActive(false);
        panelPongModeSelect.SetActive(false);
        panelStartTetris.SetActive(false);
    }

    public void ShowSelectGame()
    {
        panelMain.SetActive(false);
        panelHighScores.SetActive(false);
        panelHighScoreList.SetActive(false);
        panelSettings.SetActive(false);
        panelSelectGame.SetActive(true);
        panelPongModeSelect.SetActive(false);
        panelStartTetris.SetActive(false);
    }

    public void ShowPongModeSelect()
    {
        panelMain.SetActive(false);
        panelHighScores.SetActive(false);
        panelHighScoreList.SetActive(false);
        panelSettings.SetActive(false);
        panelSelectGame.SetActive(false);
        panelPongModeSelect.SetActive(true);
        panelStartTetris.SetActive(false);
    }

    public void ShowTetrisStartPanel()
    {
        panelMain.SetActive(false);
        panelHighScores.SetActive(false);
        panelHighScoreList.SetActive(false);
        panelSettings.SetActive(false);
        panelSelectGame.SetActive(false);
        panelPongModeSelect.SetActive(false);
        panelStartTetris.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited");
        Debug.Log("Save path: " + Application.persistentDataPath);
    }

    public void ShowHighScoreList(string gameName)
    {
        // Hide HighScores panel
        panelHighScores.SetActive(false);
        // Show HighScoreList panel
        panelHighScoreList.SetActive(true);

        // Load and display high scores for the selected game
        LoadHighScoresForGame(gameName);
    }

    public void LoadHighScoresForGame(string gameName)
    {
        textGameTitle.text = gameName + " High Scores";

        // Clear old scores
        foreach (Transform child in scoresContainer)
        {
            Destroy(child.gameObject);
        }

        // Load from file
        HighScoreData data = HighScoreManager.Instance.LoadHighScores(gameName);

        if (data.entries.Count == 0)
        {
            // Show "No scores yet" message
            GameObject scoreTextObj = new GameObject("ScoreText");
            scoreTextObj.transform.SetParent(scoresContainer, false);

            TextMeshProUGUI scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
            scoreText.fontSize = 24;
            scoreText.color = Color.white;
            scoreText.text = "No scores yet!";
            return;
        }

        // Display loaded scores
        int rank = 1;
        foreach (var entry in data.entries)
        {
            GameObject scoreTextObj = new GameObject("ScoreText");
            scoreTextObj.transform.SetParent(scoresContainer, false);

            TextMeshProUGUI scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
            scoreText.fontSize = 24;
            scoreText.color = Color.white;
            scoreText.text = $"{rank}. {entry.playerName} - {entry.score}";
            rank++;
        }
    }

    public void StartPongVsPlayer()
    {
        GameSettings.Instance.isVsAI = false;
        SceneManager.LoadScene("Pong");
    }

    public void StartPongVsAI()
    {
        GameSettings.Instance.isVsAI = true;
        SceneManager.LoadScene("Pong");
    }

    public void StartTetris()
    {
        SceneManager.LoadScene("Tetris");
    }



    
}
