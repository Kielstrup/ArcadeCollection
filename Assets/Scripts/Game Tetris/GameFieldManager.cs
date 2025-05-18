using UnityEngine;

/// <summary>
/// Manages the game field grid for Tetris-like gameplay.
/// Tracks block positions, checks and clears full rows, and detects game over conditions.
/// </summary>
public class GameFieldManager : MonoBehaviour
{
    /// <summary>
    /// Width of the grid, retrieved from Tetromino class.
    /// </summary>
    private int width => Tetromino.gridWidth;

    /// <summary>
    /// Height of the grid, retrieved from Tetromino class.
    /// </summary>
    private int height => Tetromino.gridHeight;

    /// <summary>
    /// 2D array representing the grid, storing Transform references of placed blocks.
    /// </summary>
    public Transform[,] grid;

    /// <summary>
    /// Initializes the grid array on awake.
    /// </summary>
    void Awake()
    {
        grid = new Transform[width, height];
    }

    /// <summary>
    /// Checks if a given position is within the horizontal bounds and above the bottom.
    /// </summary>
    /// <param name="pos">Position to check.</param>
    /// <returns>True if inside the grid horizontally and y >= 0; otherwise false.</returns>
    public bool IsInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    /// <summary>
    /// Rounds a Vector2 position to the nearest integer grid coordinates.
    /// </summary>
    /// <param name="pos">Position to round.</param>
    /// <returns>Rounded Vector2 position.</returns>
    public Vector2 RoundVector(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    /// <summary>
    /// Adds a block's Transform to the grid at its rounded position, if within height.
    /// </summary>
    /// <param name="block">Block transform to add.</param>
    public void AddToGrid(Transform block)
    {
        Vector2 pos = RoundVector(block.position);
        if ((int)pos.y < height)
        {
            grid[(int)pos.x, (int)pos.y] = block;
        }
    }

    /// <summary>
    /// Checks if a specific row is completely filled with blocks.
    /// </summary>
    /// <param name="y">Row index to check.</param>
    /// <returns>True if the row is full; otherwise false.</returns>
    public bool IsRowFull(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Deletes all blocks in a specified row, destroying their GameObjects and clearing references.
    /// </summary>
    /// <param name="y">Row index to delete.</param>
    public void DeleteRow(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    /// <summary>
    /// Moves all rows down starting from a given row index.
    /// Shifts blocks down by one row both logically in the grid and visually in the scene.
    /// </summary>
    /// <param name="fromY">Row index from which to start moving rows down.</param>
    public void MoveRowsDown(int fromY)
    {
        for (int y = fromY; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    // Move block down by 1 in the grid
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;

                    // Move the block's transform down visually
                    grid[x, y - 1].position += Vector3.down;
                }
            }
        }
    }

    /// <summary>
    /// Clears all full rows in the grid.
    /// Deletes full rows and shifts rows above down accordingly.
    /// </summary>
    /// <returns>The number of rows cleared.</returns>
    public int ClearFullRows()
    {
        int linesCleared = 0;

        for (int y = 0; y < height; ++y)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                MoveRowsDown(y + 1);
                y--; // Re-check the same row after shifting
                linesCleared++;
            }
        }

        return linesCleared;
    }

    /// <summary>
    /// Checks if the game is over by detecting if any blocks occupy the top row.
    /// </summary>
    /// <returns>True if the top row contains any blocks, indicating game over.</returns>
    public bool IsGameOver()
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, height - 1] != null)
            {
                return true;
            }
        }
        return false;
    }
}
