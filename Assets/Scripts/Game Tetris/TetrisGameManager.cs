using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Manages the core Tetris gameplay including spawning tetrominos,
/// handling hold mechanics, scoring, and game over flow.
/// </summary>
public class TetrisGameManager : MonoBehaviour
{
    [Header("UI & Panels")]
    public GameObject gameOverPanel;
    public TMP_InputField inputInitials;
    public GameUIManager uiManager;

    [Header("Tetromino Settings")]
    public GameObject[] tetrominoPrefabs;

    [Header("Spawn & Hold Points")]
    public Transform spawnPoint;
    public Transform holdPoint;
    public Transform nextPoint;

    [Header("Gameplay Timing")]
    public float baseFallTime = 1f;
    public float fallTimeDecrease = 0.02f;

    private int score = 0;
    public float currentFallTime;

    private GameObject currentTetromino;
    private GameObject nextTetrominoPreview;
    private GameObject nextTetromino;

    private GameObject holdTetromino;
    private bool holdUsedThisTurn = false;

    private void Start()
    {
        Time.timeScale = 1f;
        Debug.Log("Number of TetrisGameManager objects: " + FindObjectsOfType<TetrisGameManager>().Length);
        Debug.Log("TetrisGameManager Start() triggered");
        gameOverPanel.SetActive(false);
        currentFallTime = baseFallTime;

        SpawnNextTetrominoPreview();
        SpawnNewTetromino();
    }

    /// <summary>
    /// Spawns and shows the next tetromino preview (disabled so it doesn't move).
    /// </summary>
    private void SpawnNextTetrominoPreview()
    {
        int randomIndex = UnityEngine.Random.Range(0, tetrominoPrefabs.Length);
        nextTetromino = tetrominoPrefabs[randomIndex];

        if (nextTetrominoPreview != null)
            Destroy(nextTetrominoPreview);

        nextTetrominoPreview = Instantiate(nextTetromino, nextPoint.position, Quaternion.identity);
        nextTetrominoPreview.GetComponent<Tetromino>().enabled = false; // Disable behavior for preview
    }

    /// <summary>
    /// Called when the game ends to show the game over UI and reset input field.
    /// </summary>
    public void GameOver()
    {
        uiManager.ShowEndGamePanel(score);
        inputInitials.text = "";
        inputInitials.ActivateInputField();


        //Hides the preview so its not visible during game-over
        if (nextTetrominoPreview != null)
        {
            nextTetrominoPreview.SetActive(false);
        }
    }

    /// <summary>
    /// Reloads the current scene to restart the game.
    /// </summary>
    public void RestartGame()
    {
        // Full cleanup of gameplay objects
        if (currentTetromino != null) Destroy(currentTetromino);
        if (nextTetrominoPreview != null) Destroy(nextTetrominoPreview);
        if (holdTetromino != null) Destroy(holdTetromino);

        // Reset values manually in case any linger
        score = 0;
        currentFallTime = baseFallTime;
        holdUsedThisTurn = false;

        // Optional: Clear hold and next tetromino references
        nextTetromino = null;
        holdTetromino = null;

        // Reload the scene completely
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    /// <summary>
    /// Returns to the main menu scene.
    /// </summary>
    public void BackToMainMenu()
    {
        // Clean up any tetromino-related objects
        if (currentTetromino != null) Destroy(currentTetromino);
        if (nextTetrominoPreview != null) Destroy(nextTetrominoPreview);
        if (holdTetromino != null) Destroy(holdTetromino);

        // Reset all gameplay state
        score = 0;
        currentFallTime = baseFallTime;
        holdUsedThisTurn = false;
        nextTetromino = null;
        holdTetromino = null;

        // Load main menu scene
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }


    /// <summary>
    /// Spawns the current tetromino from the next tetromino, updates fall speed, and prepares the next preview.
    /// </summary>
    public void SpawnNewTetromino()
    {
        currentTetromino = Instantiate(nextTetromino, spawnPoint.position, Quaternion.identity);
        holdUsedThisTurn = false;

        Tetromino t = currentTetromino.GetComponent<Tetromino>();
        if (t != null)
        {
            Debug.Log("Spawned new Tetromino with fallTime: " + t.fallTime + ", enabled: " + t.enabled);
            t.fallTime = currentFallTime;
        }
        else
        {
            Debug.LogError("Tetromino script missing on prefab!");
        }

        // Decrease fall time but never below 0.1 seconds
        
        currentFallTime = Math.Max(0.1f, currentFallTime - fallTimeDecrease);

        SpawnNextTetrominoPreview();
    }

    /// <summary>
    /// Holds the current tetromino or swaps with the hold if already held.
    /// Only allowed once per drop.
    /// </summary>
    public void HoldCurrentTetromino()
    {
        if (holdUsedThisTurn)
            return;  // Prevent multiple holds in one turn

        if (holdTetromino == null)
        {
            // First time holding - store current and spawn new tetromino
            holdTetromino = currentTetromino;
            holdTetromino.transform.position = holdPoint.position;
            holdTetromino.SetActive(false);  // Hide held piece

            Destroy(currentTetromino);
            SpawnNewTetromino();
        }
        else
        {
            // Swap current with hold tetromino
            GameObject tempCurrent = currentTetromino;

            holdTetromino.SetActive(true);
            holdTetromino.transform.position = spawnPoint.position;

            currentTetromino = holdTetromino;

            holdTetromino = tempCurrent;
            holdTetromino.transform.position = holdPoint.position;
            holdTetromino.SetActive(false);
        }

        holdUsedThisTurn = true;
        UpdateHoldPreview();
    }

    /// <summary>
    /// Updates the hold preview visual by destroying old and instantiating new preview (disabled script).
    /// </summary>
    private void UpdateHoldPreview()
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

    /// <summary>
    /// Adds score based on lines cleared and updates UI.
    /// </summary>
    /// <param name="linesCleared">Number of lines cleared at once.</param>
    public void AddScore(int linesCleared)
    {
        score += linesCleared * 100;
        uiManager.UpdateScore(score);
    }

    public void ResetTetrisScene()
    {
        Time.timeScale = 1f; // Resume normal game speed
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

}
