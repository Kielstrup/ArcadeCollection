using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Main menu panel GameObject.
    /// </summary>
    public GameObject panelMain;

    /// <summary>
    /// Panel showing high scores options.
    /// </summary>
    public GameObject panelHighScores;

    /// <summary>
    /// Panel displaying the high score list.
    /// </summary>
    public GameObject panelHighScoreList;

    /// <summary>
    /// Settings panel GameObject.
    /// </summary>
    public GameObject panelSettings;

    /// <summary>
    /// Game selection panel GameObject.
    /// </summary>
    public GameObject panelSelectGame;

    /// <summary>
    /// Text component showing the game title on the high score list panel.
    /// </summary>
    public TextMeshProUGUI textGameTitle;

    /// <summary>
    /// Container transform to hold dynamically generated score entries.
    /// </summary>
    public Transform scoresContainer;

    /// <summary>
    /// Optional prefab for score text items.
    /// </summary>
    public GameObject scoreTextPrefab;

    /// <summary>
    /// Pong mode selection panel.
    /// </summary>
    public GameObject panelPongModeSelect;

    /// <summary>
    /// Tetris start panel.
    /// </summary>
    public GameObject panelStartTetris;

    /// <summary>
    /// Shows the main menu panel and hides all other panels.
    /// </summary>
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

    /// <summary>
    /// Shows the high scores panel and hides all other panels.
    /// </summary>
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

    /// <summary>
    /// Shows the settings panel and hides all other panels.
    /// </summary>
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

    /// <summary>
    /// Shows the game selection panel and hides all other panels.
    /// </summary>
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

    /// <summary>
    /// Shows the Pong mode selection panel and hides all other panels.
    /// </summary>
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

    /// <summary>
    /// Shows the Tetris start panel and hides all other panels.
    /// </summary>
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

    /// <summary>
    /// Quits the application. Logs exit message and save path in the editor.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited");
        Debug.Log("Save path: " + Application.persistentDataPath);
    }

    /// <summary>
    /// Shows the high score list panel for the specified game and hides the high scores overview panel.
    /// </summary>
    /// <param name="gameName">Name of the game to display high scores for.</param>
    public void ShowHighScoreList(string gameName)
    {
        panelHighScores.SetActive(false);
        panelHighScoreList.SetActive(true);

        LoadHighScoresForGame(gameName);
    }

    /// <summary>
    /// Loads and displays the high scores for a specific game.
    /// </summary>
    /// <param name="gameName">The game name to load high scores from.</param>
    public void LoadHighScoresForGame(string gameName)
    {
        textGameTitle.text = gameName + " High Scores";

        // Clear old scores
        foreach (Transform child in scoresContainer)
        {
            Destroy(child.gameObject);
        }

        HighScoreData data = HighScoreManager.Instance.LoadHighScores(gameName);

        if (data.entries.Count == 0)
        {
            GameObject scoreTextObj = new GameObject("ScoreText");
            scoreTextObj.transform.SetParent(scoresContainer, false);

            TextMeshProUGUI scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
            scoreText.fontSize = 24;
            scoreText.color = Color.white;
            scoreText.text = "No scores yet!";
            return;
        }

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

    /// <summary>
    /// Starts Pong game in player-vs-player mode.
    /// </summary>
    public void StartPongVsPlayer()
    {
        GameSettings.Instance.isVsAI = false;
        SceneManager.LoadScene("Pong");
    }

    /// <summary>
    /// Starts Pong game in player-vs-AI mode.
    /// </summary>
    public void StartPongVsAI()
    {
        GameSettings.Instance.isVsAI = true;
        SceneManager.LoadScene("Pong");
    }

    /// <summary>
    /// Starts the Tetris game scene.
    /// </summary>
    public void StartTetris()
    {
        SceneManager.LoadScene("Tetris");
    }
}
