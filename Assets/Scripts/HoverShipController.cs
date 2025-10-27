using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class HoverShipController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float hoverHeight = 1.5f;
    public float hoverAmplitude = 0.1f;
    public float hoverFrequency = 3f;

    private Rigidbody rb;
    private float baseY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        baseY = hoverHeight;
    }

    void FixedUpdate()
    {
        // Get input for free movement (WASD or Arrow Keys)
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down

        // Combine input into a movement vector (relative to ship's facing)
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Move the ship in world space
        Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Optional: make the ship face the movement direction (for visual effect)
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, toRotation, 0.2f));
        }

        // Hover effect (bobbing)
        float hoverOffset = Mathf.Sin(Time.time * hoverFrequency) * hoverAmplitude;
        Vector3 targetPos = new Vector3(rb.position.x, baseY + hoverOffset, rb.position.z);
        rb.position = targetPos;
    }
}
