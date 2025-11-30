using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Bullet properties
    public float speed = 30f;       // Movement speed of the bullet
    public float lifeTime = 3f;     // Time before the bullet automatically despawns
    public int damage = 1;          // Damage dealt to the player

    [HideInInspector]
    public Vector3 moveDir;         // Direction the bullet travels (assigned when fired)

    void Start()
    {
        // Destroy bullet after a set lifetime to avoid buildup
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move the bullet along its assigned direction every frame
        transform.position += moveDir * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hit the player
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            // Apply damage to the player
            player.TakeDamage();

            // Destroy bullet on impact
            Destroy(gameObject);
        }
    }
}
