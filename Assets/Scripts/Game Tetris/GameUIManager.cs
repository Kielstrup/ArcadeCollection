using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject endGamePanel;
    public TextMeshProUGUI finalScoreText;

    private int finalScore = 0;
    public TextMeshProUGUI scoreText;

    public TMP_InputField initialsInputField;

    void Start()
    {
        endGamePanel.SetActive(false);
    }

    public void ShowEndGamePanel(int score)
    {
        finalScore = score;
        endGamePanel.SetActive(true);
        finalScoreText.text = $"Final Score: {finalScore}";
        Time.timeScale = 0f;
    }

    public void OnSaveAndRestartPressed()
    {
        string playerInitials = initialsInputField.text;

        if (playerInitials.Length != 3)
        {
            
            Debug.LogWarning("Please enter exactly 3 letters for your initials.");
            return;
        }

        HighScoreManager.Instance.AddHighScore("Tetris", playerInitials, finalScore);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnSaveAndQuitPressed()
    {
        string playerInitials = initialsInputField.text;

        if (playerInitials.Length != 3)
        {
            Debug.LogWarning("Please enter exactly 3 letters for your initials.");
            return;
        }

        HighScoreManager.Instance.AddHighScore("Tetris", playerInitials, finalScore);

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }
    
}
