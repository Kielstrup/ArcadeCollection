using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages UI elements for the game including displaying scores, handling end game panel,
/// and saving high scores with player initials.
/// </summary>
public class GameUIManager : MonoBehaviour
{
    /// <summary>
    /// Panel shown at the end of the game.
    /// </summary>
    public GameObject endGamePanel;

    /// <summary>
    /// Text component to show the final score.
    /// </summary>
    public TextMeshProUGUI finalScoreText;

    /// <summary>
    /// Holds the player's final score when the game ends.
    /// </summary>
    private int finalScore = 0;

    /// <summary>
    /// Text component to show the current score during gameplay.
    /// </summary>
    public TextMeshProUGUI scoreText;

    /// <summary>
    /// Input field for entering player initials (expected exactly 3 letters).
    /// </summary>
    public TMP_InputField initialsInputField;

    /// <summary>
    /// Initializes the UI state by hiding the end game panel.
    /// </summary>
    void Start()
    {
        endGamePanel.SetActive(false);
    }

    /// <summary>
    /// Displays the end game panel with the final score and pauses the game.
    /// </summary>
    /// <param name="score">The final score to display.</param>
    public void ShowEndGamePanel(int score)
    {
        finalScore = score;
        endGamePanel.SetActive(true);
        finalScoreText.text = $"Final Score: {finalScore}";
        Time.timeScale = 0f; // Pause the game
    }

    /// <summary>
    /// Called when player presses save & restart button.
    /// Validates initials and saves the score, then reloads the current scene.
    /// </summary>
    public void OnSaveAndRestartPressed()
    {
        string playerInitials = initialsInputField.text;

        if (playerInitials.Length != 3)
        {
            Debug.LogWarning("Please enter exactly 3 letters for your initials.");
            return;
        }

        HighScoreManager.Instance.AddHighScore("Tetris", playerInitials, finalScore);

        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Called when player presses save & quit button.
    /// Validates initials and saves the score, then loads the main menu.
    /// </summary>
    public void OnSaveAndQuitPressed()
    {
        string playerInitials = initialsInputField.text;

        if (playerInitials.Length != 3)
        {
            Debug.LogWarning("Please enter exactly 3 letters for your initials.");
            return;
        }

        HighScoreManager.Instance.AddHighScore("Tetris", playerInitials, finalScore);

        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Updates the score display during gameplay.
    /// </summary>
    /// <param name="score">Current score value.</param>
    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
}
