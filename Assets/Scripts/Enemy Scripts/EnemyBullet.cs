using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 3f;
    public int damage = 1;

    [HideInInspector]
    public Vector3 moveDir;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Always move forward in the assigned direction
        transform.position += moveDir * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage();
            Destroy(gameObject);
        }
    }
}
