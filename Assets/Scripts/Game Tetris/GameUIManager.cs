using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public GameObject endGamePanel;
    public TextMeshProUGUI finalScoreText;

    private int finalScore = 0;

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
        Time.timeScale = 1f;
        //TODO: Add saving
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnSaveAndQuitPressed()
    {
        Time.timeScale = 1f;
        //TODO: Add saving
        SceneManager.LoadScene("MainMenu");
    }
    
}
