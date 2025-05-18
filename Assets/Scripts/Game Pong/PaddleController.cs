using UnityEngine;

/// <summary>
/// Controls the paddle movement for player or AI in the Pong game.
/// Supports player input via input axes and simple AI following the ball's Y position.
/// </summary>
public class PaddleController : MonoBehaviour
{
    /// <summary>Movement speed of the paddle.</summary>
    public float speed = 10f;

    /// <summary>Input axis name for player control (e.g., "Vertical" or "Vertical2").</summary>
    public string inputAxis;

    private Rigidbody2D rb;

    /// <summary>Flag indicating whether this paddle is controlled by AI.</summary>
    public bool isAI = false;

    /// <summary>Reference to the ball transform (for AI tracking).</summary>
    public Transform ball;

    /// <summary>Movement speed factor for the AI paddle.</summary>
    public float aiSpeed = 4f;

    private float currentMove = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Freeze horizontal movement and rotation to restrict paddle movement to vertical axis only
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        // Set AI only for the right paddle based on global GameSettings
        if (gameObject.name == "PaddleRight")
        {
            isAI = GameSettings.Instance.isVsAI;

            if (isAI)
            {
                GameObject ballObj = GameObject.FindGameObjectWithTag("Ball");
                if (ballObj != null)
                {
                    ball = ballObj.transform;
                }
                else
                {
                    Debug.LogWarning("AI paddle could not find ball with tag 'Ball'");
                }
            }
        }
        else
        {
            isAI = false;
        }
    }

    /// <summary>
    /// Updates paddle position based on player input or AI logic.
    /// </summary>
    [System.Obsolete] 
    void Update()
    {
        Debug.Log($"[{gameObject.name}] isAI: {isAI}, ball: {(ball == null ? "null" : ball.name)}");

        // Try to reacquire the ball if AI loses reference (e.g., scene reloads)
        if (isAI && ball == null)
        {
            GameObject ballObj = GameObject.FindGameObjectWithTag("Ball");
            if (ballObj != null)
            {
                ball = ballObj.transform;
                Debug.Log("AI paddle found the ball during Update!");
            }
        }

        float move = 0f;

        if (isAI)
        {
            if (ball != null)
            {
                // Determine movement direction by comparing ball Y position with paddle position
                float direction = 0f;
                float threshold = 0.1f;

                if (ball.position.y > transform.position.y + threshold)
                    direction = 1f;
                else if (ball.position.y < transform.position.y - threshold)
                    direction = -1f;
                else
                    direction = 0f;

                // Smoothly interpolate current velocity towards desired direction scaled by AI speed
                move = Mathf.Lerp(rb.velocity.y / speed, direction, Time.deltaTime * aiSpeed);
            }
        }
        else
        {
            // Player-controlled movement from input axis
            move = Input.GetAxisRaw(inputAxis);
            Debug.Log("Is AI: " + GameSettings.Instance.isVsAI);
        }

        // Apply vertical velocity, horizontal velocity is always zero
        Vector2 velocity = new Vector2(0, move * speed);
        rb.velocity = velocity;
    }

    void LateUpdate()
    {
        // Get vertical bounds of the camera in world units (orthographic size)
        float cameraHeight = Camera.main.orthographicSize;

        // Calculate half the paddle height (scale.y assumes uniform scale)
        float paddleHeight = transform.localScale.y / 2;

        // Clamp the paddle's vertical position within camera bounds
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -cameraHeight + paddleHeight, cameraHeight - paddleHeight);
        transform.position = pos;
    }
}
