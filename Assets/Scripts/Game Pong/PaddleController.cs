using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;
    public string inputAxis; // "Vertical" or "Vertical2"

    private Rigidbody2D rb;

    public bool isAI = false;
    public Transform ball;
    public float aiSpeed = 4f;
    private float currentMove = 0f;


    void Start()
{
    rb = GetComponent<Rigidbody2D>();
    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

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


    [System.Obsolete]
    
    void Update()
    {
        Debug.Log($"[{gameObject.name}] isAI: {isAI}, ball: {(ball == null ? "null" : ball.name)}");

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
                float direction = 0f;
                float threshold = 0.1f;

                if (ball.position.y > transform.position.y + threshold)
                    direction = 1f;
                else if (ball.position.y < transform.position.y - threshold)
                    direction = -1f;
                else
                    direction = 0f;

                // Slow down the AI movement by multiplying with aiSpeed instead of speed
                move = Mathf.Lerp(rb.velocity.y / speed, direction, Time.deltaTime * aiSpeed);
            }
        }
        else
        {
            move = Input.GetAxisRaw(inputAxis);
            Debug.Log("Is AI: " + GameSettings.Instance.isVsAI);
        }

        Vector2 velocity = new Vector2(0, move * speed);
        rb.velocity = velocity;
    }


    void LateUpdate()
    {
        // Get camera vertical boundaries in world units
        float cameraHeight = Camera.main.orthographicSize;
        float paddleHeight = transform.localScale.y / 2;

        // Clamp the paddle's Y position so it stays within the camera view
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -cameraHeight + paddleHeight, cameraHeight - paddleHeight);
        transform.position = pos;
    }
    

}
