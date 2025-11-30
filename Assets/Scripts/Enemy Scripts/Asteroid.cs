using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public int scoreValue = 10;

    public float moveSpeed = 2f;
    private Vector3 direction;

    public float baseRotationSpeed = 2f;
    public float sizeMultiplier = 1f;

    private Rigidbody rb;

    [Header("Audio")]
    public AudioSource hitAudio;     // plays hit sounds normally
    public AudioClip deathSound;     // explosion SFX
    public GameObject deathSoundPrefab; // plays death sound after asteroid is destroyed

    [Header("Explosion VFX")]
    public GameObject explosionPrefab;  // drag your explosion effect here

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        rb.constraints = RigidbodyConstraints.FreezePositionY;

        currentHealth = maxHealth;

        float x = Random.Range(-1f, 1f);
        float z = Random.Range(-1f, 1f);
        direction = new Vector3(x, 0f, z).normalized;

        StartCoroutine(ApplySpinDelayed());
    }

    IEnumerator ApplySpinDelayed()
    {
        yield return new WaitForFixedUpdate();

        float finalSpin = baseRotationSpeed / sizeMultiplier;
        rb.angularVelocity = Random.insideUnitSphere * finalSpin;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            ContactPoint contact = collision.contacts[0];
            direction = Vector3.Reflect(direction, contact.normal);
            direction.y = 0f;
            direction.Normalize();
        }

        PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // play hit sound
        if (hitAudio != null)
            hitAudio.Play();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Spawn explosion effect
        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 3f); // Adjust to match your particle duration
        }

        // Spawn temporary audio object for death sound
        if (deathSoundPrefab != null && deathSound != null)
        {
            GameObject snd = Instantiate(deathSoundPrefab, transform.position, Quaternion.identity);
            AudioSource src = snd.GetComponent<AudioSource>();
            src.PlayOneShot(deathSound);
            Destroy(snd, deathSound.length);
        }

        // Add score
        ScoreManager.instance.AddScore(scoreValue);

        // Destroy asteroid
        Destroy(gameObject);
    }
}
