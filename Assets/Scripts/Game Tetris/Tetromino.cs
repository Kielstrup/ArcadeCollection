using UnityEngine;

public class Tetromino : MonoBehaviour
{
    #region Fields & Properties

    /// <summary>
    /// Time interval between automatic downward movements.
    /// </summary>
    public float fallTime = 1f;

    private float previousTime;

    /// <summary>
    /// Width of the game grid.
    /// </summary>
    public static int gridWidth = 10;

    /// <summary>
    /// Height of the game grid.
    /// </summary>
    public static int gridHeight = 20;

    private TetrisGameManager gameManager;
    private GameFieldManager fieldManager;

    #endregion


    #region Unity Callbacks

    /// <summary>
    /// Called once per frame. Handles falling and input for the tetromino.
    /// </summary>
    void Update()
    {
        if (Time.time - previousTime > fallTime)
        {
            transform.position += Vector3.down;

            if (!IsValidMove())
            {
                transform.position += Vector3.up;
                LockTetromino();
                enabled = false;
            }

            previousTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            fallTime = 0.05f;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            fallTime = gameManager != null ? gameManager.currentFallTime : 1f;
        }
    }

    /// <summary>
    /// Initializes references to the game and field managers.
    /// </summary>
    void Start()
    {
        gameManager = FindObjectOfType<TetrisGameManager>();
        fieldManager = FindObjectOfType<GameFieldManager>();
    }

    #endregion


    #region Movement & Rotation

    /// <summary>
    /// Moves the tetromino in the specified direction if the move is valid.
    /// </summary>
    /// <param name="direction">Direction to move the tetromino.</param>
    void Move(Vector3 direction)
    {
        transform.position += direction;
        if (!IsValidMove())
        {
            transform.position -= direction;
        }
    }

    /// <summary>
    /// Rotates the tetromino 90 degrees clockwise and attempts wall kicks if needed.
    /// </summary>
    void Rotate()
    {
        transform.Rotate(0f, 0f, 90f);

        if (!IsValidMove())
        {
            // Try wall kick left
            transform.position += Vector3.left;
            if (IsValidMove()) return;

            // Try wall kick right
            transform.position += Vector3.right * 2; // total of +1 from original
            if (IsValidMove()) return;

            // Undo wall kicks
            transform.position += Vector3.left;

            // Final fallback: undo rotation
            transform.Rotate(0f, 0f, -90f);
        }
    }

    #endregion


    #region Validation & Locking

    /// <summary>
    /// Checks if the tetromino is currently in a valid position within the grid.
    /// </summary>
    /// <returns>True if valid, false if invalid.</returns>
    bool IsValidMove()
    {
        foreach (Transform block in transform)
        {
            Vector2 pos = fieldManager.RoundVector(block.position);

            if (!fieldManager.IsInsideGrid(pos))
            {
                return false;
            }
            if (fieldManager.grid[(int)pos.x, (int)pos.y] != null)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Locks the tetromino into the grid, clears full rows, updates the score, and spawns a new tetromino or ends the game.
    /// </summary>
    void LockTetromino()
    {
        foreach (Transform block in transform)
        {
            Vector2 pos = fieldManager.RoundVector(block.position);
            if ((int)pos.y < gridHeight)
            {
                fieldManager.grid[(int)pos.x, (int)pos.y] = block;
            }
        }

        int linesCleared = fieldManager.ClearFullRows();
        gameManager.AddScore(linesCleared);

        if (fieldManager.IsGameOver())
        {
            gameManager.GameOver();
        }
        else
        {
            gameManager.SpawnNewTetromino();
        }

        Destroy(this);
    }

    #endregion
}
