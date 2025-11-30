using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    // Asteroid health and score value when destroyed
    public int maxHealth = 3;
    private int currentHealth;
    public int scoreValue = 10;

    // Movement variables
    public float moveSpeed = 2f;
    private Vector3 direction;

    // Rotation control (larger asteroids rotate slower)
    public float baseRotationSpeed = 2f;
    public float sizeMultiplier = 1f;

    private Rigidbody rb;

    [Header("Audio")]
    public AudioSource hitAudio;           // Sound played when asteroid is damaged
    public AudioClip deathSound;           // Explosion sound played on destruction
    public GameObject deathSoundPrefab;    // Temporary audio object used so sound continues after asteroid is destroyed

    [Header("Explosion VFX")]
    public GameObject explosionPrefab;     // Explosion visual effect spawned on destruction

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Configure rigidbody for floating, physics-based movement
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezePositionY;

        currentHealth = maxHealth;

        // Choose a random movement direction on the XZ plane
        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        direction = new Vector3(x, 0f, z).normalized;

        // Delay rotation application by 1 physics frame to ensure stability
        StartCoroutine(ApplySpinDelayed());
    }

    IEnumerator ApplySpinDelayed()
    {
        yield return new WaitForFixedUpdate();

        // Calculate rotation speed depending on asteroid size
        float finalSpin = baseRotationSpeed / sizeMultiplier;

        // Apply random rotational velocity
        rb.angularVelocity = Random.insideUnitSphere * finalSpin;
    }

    void FixedUpdate()
    {
        // Move asteroid continuously using physics
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Bounce off walls using reflection physics
        if (collision.collider.CompareTag("Wall"))
        {
            ContactPoint contact = collision.contacts[0];
            direction = Vector3.Reflect(direction, contact.normal);
            direction.y = 0f;
            direction.Normalize();
        }

        // Damage the player if the asteroid collides with them
        PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Play hit sound feedback
        if (hitAudio != null)
            hitAudio.Play();

        // If health reaches zero, destroy the asteroid
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Spawn explosion visual effect at asteroid position
        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 3f); // Remove effect after duration
        }

        // Spawn temporary object to play explosion sound without interruption
        if (deathSoundPrefab != null && deathSound != null)
        {
            GameObject snd = Instantiate(deathSoundPrefab, transform.position, Quaternion.identity);
            AudioSource src = snd.GetComponent<AudioSource>();
            src.PlayOneShot(deathSound);
            Destroy(snd, deathSound.length);
        }

        // Award points to the player
        ScoreManager.instance.AddScore(scoreValue);

        // Remove asteroid from the scene
        Destroy(gameObject);
    }
}
