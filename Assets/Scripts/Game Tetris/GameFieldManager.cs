using UnityEngine;

public class GameFieldManager : MonoBehaviour
{
    int width = Tetromino.gridWidth;

    int height = Tetromino.gridHeight;

    public Transform[,] grid;

    void Awake()
    {
        grid = new Transform[width, height];
    }

    public bool IsInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    public Vector2 RoundVector(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    public void AddToGrid(Transform block)
    {
        Vector2 pos = RoundVector(block.position);
        if (pos.y > height)
        {
            grid[(int)pos.x, (int)pos.y] = block;
        }
    }

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

    public void DeleteRow(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowsDown(int startY)
    {
        for (int y = startY; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y - 1] = grid[x, y];
                    grid[x, y] = null;
                    grid[x, y - 1].position += Vector3.down;
                }
            }
        }
    }

    public void ClearFullRows()
    {
        for (int y = 0; y < height; ++y)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                MoveRowsDown(y + 1);
                y--;
            }
        }
    }
}
