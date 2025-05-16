using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;
    public string inputAxis; // "Vertical" or "Vertical2"

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    [System.Obsolete]
    void Update()
    {
        float move = Input.GetAxisRaw(inputAxis);
        Debug.Log($"{gameObject.name} input: {move}");
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
