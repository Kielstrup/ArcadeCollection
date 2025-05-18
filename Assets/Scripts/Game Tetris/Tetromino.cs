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

    void Update()
    {
        if (Time.time - previousTime > fallTime)
        {
            transform.position += new UnityEngine.Vector3(0, -1, 0);

            if (!IsValidMove())
            {
                transform.position += new UnityEngine.Vector3(0, 1, 0);
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
            fallTime = 0.0f;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            fallTime = 1f;
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
            transform.Rotate(0f, 0f, 90f);
        }

    }

    bool IsValidMove() {
        return true;
    }
}
