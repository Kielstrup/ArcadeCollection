using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisGameManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    public GameUIManager uiManager;

    public GameObject[] tetrominoPrefabs;

    private int score = 0;

    private GameObject currentTetromino;
    private GameObject nextTetrominoPreview;
    private GameObject nextTetromino;

    private GameObject holdTetromino;
    private bool holdUsedThisTurn = false;

    public Transform spawnPoint;
    public Transform holdPoint;
    public Transform nextPoint;

    private void Start()
    {
        gameOverPanel.SetActive(false);

        SpawnNextTetrominoPreview();

        SpawnNewTetromino();
    }

    void SpawnNextTetrominoPreview()
    {
        int randomIndex = Random.Range(0, tetrominoPrefabs.Length);
        nextTetromino = tetrominoPrefabs[randomIndex];

        if (nextTetrominoPreview != null)
        {
            Destroy(nextTetrominoPreview);
        }

        nextTetrominoPreview = Instantiate(nextTetromino, nextPoint.position, Quaternion.identity);
        nextTetrominoPreview.GetComponent<Tetromino>().enabled = false;
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
        currentTetromino = Instantiate(nextTetromino, spawnPoint.position, quaternion.identity);
        holdUsedThisTurn = false;

        SpawnNextTetrominoPreview();
    }

    public void HoldCurrentTetromino()
    {
        if (holdUsedThisTurn) return;

        if (holdTetromino == null)
        {
            holdTetromino = currentTetromino;
            holdTetromino.transform.position == holdPoint.position;
            holdTetromino.SetActive(false);

            Destroy(currentTetromino);
            SpawnNewTetromino();
        }
        else
        {
            GameObject temp = holdTetromino;

            holdTetromino = currentTetromino;
            holdTetromino.transform.position = holdPoint.position;
            holdTetromino.SetActive(false);

            Destroy(currentTetromino);

            currentTetromino = Instantiate(temp, spawnPoint.position, Quaternion.identity);
        }

        holdUsedThisTurn = true;
        UpdateHoldPreview();
    }

    void UpdateHoldPreview()
    {
        foreach (Transform child in holdPoint)
        {
            Destroy(child.gameObject);
        }
        if (holdTetromino != null)
        {
            GameObject preview = Instantiate(holdTetromino, holdPoint.position, Quaternion.identity);
            preview.GetComponent<Tetromino>().enabled = false;
            preview.SetActive(true);
        }
    }

    public void AddScore(int linesCleared)
    {
        score += linesCleared * 100;
        uiManager.UpdateScore(score);
    }
}
