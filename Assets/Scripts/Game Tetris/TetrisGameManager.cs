using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisGameManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    public GameUIManager uiManager;

    public GameObject[] tetrominoPrefabs;

    private int score = 0;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        SpawnNewTetromino();
    }

    public void GameOver()
    {
        uiManager.ShowEndGamePanel(score);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void SpawnNewTetromino()
    {
        int spawnX = Tetromino.gridWidth / 2;
        int spawnY = Tetromino.gridHeight;
        int randomIndex = Random.Range(0, tetrominoPrefabs.Length);
        Instantiate(tetrominoPrefabs[randomIndex], new UnityEngine.Vector3(spawnX, spawnY, 0), Quaternion.identity);
    }

    public void AddScore(int linesCleared)
    {
        score += linesCleared * 100;
        uiManager.UpdateScore(score);
    }
}
