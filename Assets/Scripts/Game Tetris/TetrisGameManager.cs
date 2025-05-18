using System;
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

    public float baseFallTime = 1f;

    public float fallTimeDecrease = 0.05f;
    private float currentFallTime;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        currentFallTime = baseFallTime;

        SpawnNextTetrominoPreview();

        SpawnNewTetromino();
    }

    void SpawnNextTetrominoPreview()
    {
        int randomIndex = UnityEngine.Random.Range(0, tetrominoPrefabs.Length);
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
        currentTetromino = Instantiate(nextTetromino, spawnPoint.position,Quaternion.identity);
        holdUsedThisTurn = false;

        Tetromino t = currentTetromino.GetComponent<Tetromino>();
        if (t != null)
        {
            t.fallTime = currentFallTime;
        }

        currentFallTime = Math.Max(0.1f, currentFallTime - fallTimeDecrease);

        SpawnNextTetrominoPreview();
    }

    public void HoldCurrentTetromino()
    {
        if (holdUsedThisTurn) return;  // Prevent multiple holds per drop

        if (holdTetromino == null)
        {
            // No hold yet, store current tetromino
            holdTetromino = currentTetromino;
            holdTetromino.transform.position = holdPoint.position;
            holdTetromino.gameObject.SetActive(false);  // hide in hold spot

            // Spawn new tetromino from next
            Destroy(currentTetromino);
            SpawnNewTetromino();
        }
        else
        {
            // Swap current and hold tetromino

            // Store reference to current
            GameObject tempCurrent = currentTetromino;

            // Reactivate hold piece and move to spawn
            holdTetromino.SetActive(true);
            holdTetromino.transform.position = spawnPoint.position;

            // Set current tetromino to hold piece
            currentTetromino = holdTetromino;

            // Store previous current as hold piece, hide and move to hold position
            holdTetromino = tempCurrent;
            holdTetromino.transform.position = holdPoint.position;
            holdTetromino.SetActive(false);
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
