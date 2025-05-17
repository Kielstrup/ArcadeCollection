using System.ComponentModel;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 8f;
    public float speedIncreaseFactor = 1.05f;
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

    [System.Obsolete]
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Paddle"))
        {
            Transform paddle = collision.transform;
            float paddleHeight = paddle.GetComponent<BoxCollider2D>().bounds.size.y;

            float y = HitFactor(transform.position, paddle.position, paddleHeight);
            float x = (transform.position.x < 0) ? 1 : -1;

            Vector2 dir = new Vector2(x, y).normalized;
            rb.velocity = dir * (rb.velocity.magnitude * 1.05f);

        }
    }


    float HitFactor(Vector2 ballPos, Vector2 paddlePos, float paddleHeight)
    {
        return (ballPos.y - paddlePos.y) / paddleHeight;
    
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
