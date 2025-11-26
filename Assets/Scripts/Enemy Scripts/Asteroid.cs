using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public float moveSpeed = 2f;
    private Vector3 direction;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Rigidbody setup
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        currentHealth = maxHealth;

        // random direction on XZ plane
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        direction = new Vector3(x, 0f, z).normalized;
    }

    void FixedUpdate()
    {
        // Move using physics so collisions WORK
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Bounce if we hit a wall
        if (collision.collider.CompareTag("Wall"))
        {
            ContactPoint contact = collision.contacts[0];

            // Reflect direction based on wall surface normal
            direction = Vector3.Reflect(direction, contact.normal);

            // keep movement flat
            direction.y = 0f;
            direction.Normalize();
        }

        // Damage player
        PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
