using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody2D rb;

    [System.Obsolete]
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchBall();
    }

    [System.Obsolete]
    public void LaunchBall()
    {
        // Random initial direction
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(-1f, 1f);
        Vector2 direction = new Vector2(x, y).normalized;

        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Optionally add some spin or change velocity based on where it hit the paddle
    }

    [System.Obsolete]
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
