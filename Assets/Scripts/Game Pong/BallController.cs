using System.ComponentModel;
using UnityEngine;

/// <summary>
/// Controls the ball behavior in the Pong game,
/// including launching, bouncing off paddles with angle calculation,
/// speed increases on paddle hits, and detecting goals.
/// </summary>
public class BallController : MonoBehaviour
{
    /// <summary>Initial speed of the ball.</summary>
    public float speed = 8f;

    /// <summary>Factor to increase the ball's speed on paddle hit.</summary>
    public float speedIncreaseFactor = 1.05f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchBall();
    }

    /// <summary>
    /// Launches the ball in a random direction only if the game has started.
    /// </summary>
    public void LaunchBall()
    {
        // Check if the game has started before launching the ball
        if (!FindAnyObjectByType<PongGameManager>().isGameStarted)
        {
            return;
        }

        // Randomly pick left or right horizontal direction
        float x = Random.Range(0, 2) == 0 ? -1 : 1;

        // Random vertical direction between -1 and 1
        float y = Random.Range(-1f, 1f);

        // Normalize to ensure consistent speed magnitude
        Vector2 direction = new Vector2(x, y).normalized;

        // Set the ball velocity
        rb.linearVelocity = direction * speed;
    }

    /// <summary>
    /// Handles collision with paddles by calculating bounce angle and increasing speed.
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            Transform paddle = collision.transform;
            float paddleHeight = paddle.GetComponent<BoxCollider2D>().bounds.size.y;

            // Calculate hit factor for the ball’s new vertical direction
            float y = HitFactor(transform.position, paddle.position, paddleHeight);

            // Determine horizontal direction based on ball position (left or right side)
            float x = (transform.position.x < 0) ? 1 : -1;

            // Create new direction vector and normalize
            Vector2 dir = new Vector2(x, y).normalized;

            // Increase ball speed by speedIncreaseFactor on each paddle hit
            rb.linearVelocity = dir * (rb.linearVelocity.magnitude * speedIncreaseFactor);
        }
    }

    /// <summary>
    /// Calculates the relative hit position on the paddle to adjust ball angle.
    /// </summary>
    /// <param name="ballPos">Current ball position.</param>
    /// <param name="paddlePos">Current paddle position.</param>
    /// <param name="paddleHeight">Height of the paddle collider.</param>
    /// <returns>A float between -0.5 and 0.5 representing relative vertical hit position.</returns>
    float HitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleHeight)
    {
        return (ballPos.y - paddlePos.y) / paddleHeight;
    }

    /// <summary>
    /// Detects when the ball enters a goal trigger and updates the score accordingly.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GoalLeft"))
        {
            Debug.Log("Ball entered GoalLeft");
            FindObjectOfType<PongGameManager>().ScoreRight();
        }
        else if (other.CompareTag("GoalRight"))
        {
            Debug.Log("Ball entered GoalRight");
            FindObjectOfType<PongGameManager>().ScoreLeft();
        }
    }
}
