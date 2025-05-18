using System.Numerics;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

public class Tetromino : MonoBehaviour
{
    public float fallTime = 1f;
    private float previousTime;
    public static int gridWidth = 10;
    public static int gridHeight = 20;
    private TetrisGameManager gameManager;
    private GameFieldManager fieldManager;

    void Update()
    {
        if (Time.time - previousTime > fallTime)
        {
            transform.position += UnityEngine.Vector3.down;

            if (!IsValidMove())
            {
                transform.position += UnityEngine.Vector3.up;
                LockTetromino();
                enabled = false;
            }

            previousTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(UnityEngine.Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(UnityEngine.Vector3.right);
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
            if (gameManager != null)
            {
                fallTime = gameManager.currentFallTime;
            }
            else
            {
                fallTime = 1f;
            }
        }
    
    }

    void Move(UnityEngine.Vector3 direction)
    {
        transform.position += direction;
        if (!IsValidMove())
        {
            transform.position -= direction;
        }
    }

    void Rotate()
    {
        transform.Rotate(0f, 0f, 90f);

        if (!IsValidMove())
        {
            // Try wall kick left
            transform.position += UnityEngine.Vector3.left;
            if (IsValidMove()) return;

            // Try wall kick right
            transform.position += UnityEngine.Vector3.right * 2; // total of +1 from original
            if (IsValidMove()) return;

            // Undo wall kicks
            transform.position += UnityEngine.Vector3.left;

            // Final fallback: undo rotation
            transform.Rotate(0f, 0f, -90f);
        }
    }


    bool IsValidMove()
    {
        foreach (Transform block in transform)
        {
            UnityEngine.Vector2 pos = fieldManager.RoundVector(block.position);

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

    void LockTetromino()
    {
        foreach (Transform block in transform)
        {
            UnityEngine.Vector2 pos = fieldManager.RoundVector(block.position);
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

    void Start()
    {
        gameManager = FindObjectOfType<TetrisGameManager>();
        fieldManager = FindObjectOfType<GameFieldManager>();
    }
}
