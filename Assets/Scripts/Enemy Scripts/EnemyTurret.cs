using UnityEngine;
using System.Collections;

public class EnemyTurret : MonoBehaviour
{
    private Transform player;                // AUTO-DETECTED player
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float rotationSpeed = 5f;

    public int burstCount = 3;
    public float burstDelay = 0.2f;
    public float burstCooldown = 4f;

    public AudioSource shootAudio;

    private bool isFiring = false;

    void Start()
    {
        // Automatically find the REAL player in the scene
        GameObject p = GameObject.FindWithTag("Player");

        if (p != null)
            player = p.transform;
        else
            Debug.LogError("EnemyTurret could NOT find Player (tag missing?)");
    }

    void Update()
    {
        if (!player) return;

        // Rotate toward the player
        Vector3 direction = (player.position - transform.position);
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Start firing if not already firing
        if (!isFiring)
        {
            StartCoroutine(BurstFire());
        }
    }

    IEnumerator BurstFire()
    {
        isFiring = true;

        for (int i = 0; i < burstCount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(burstDelay);
        }

        yield return new WaitForSeconds(burstCooldown);
        isFiring = false;
    }

    void Shoot()
    {
        if (!bulletPrefab || !player) return;

        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, transform.rotation);

        EnemyBullet bullet = bulletObj.GetComponent<EnemyBullet>();

        // Compute direction toward player **AT THE MOMENT OF SHOOTING**
        Vector3 dir = (player.position - transform.position);
        dir.y = 0f;

        if (bullet != null)
        {
            bullet.moveDir = dir.normalized;
            bullet.speed = bulletSpeed;
        }

        if (shootAudio != null)
            shootAudio.Play();
    }
}
