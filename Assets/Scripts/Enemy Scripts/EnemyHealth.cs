using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public int scoreValue = 100;

    [Header("Audio")]
    public AudioSource hitAudio;          // plays hit sound
    public AudioClip deathSound;          // explosion sound
    public GameObject deathSoundPrefab;   // prefab with only AudioSource

    [Header("Explosion VFX")]
    public GameObject explosionPrefab;    // explosion for enemies

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // Play hit sound
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
            Destroy(fx, 3f); // Adjust depending on your explosion duration
        }

        // Play explosion sound using temporary audio object
        if (deathSoundPrefab != null && deathSound != null)
        {
            GameObject snd = Instantiate(deathSoundPrefab, transform.position, Quaternion.identity);
            AudioSource src = snd.GetComponent<AudioSource>();

            src.PlayOneShot(deathSound);
            Destroy(snd, deathSound.length);
        }

        // Add score
        ScoreManager.instance.AddScore(scoreValue);

        // Destroy enemy
        Destroy(gameObject);
    }
}
