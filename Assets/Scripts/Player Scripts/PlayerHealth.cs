using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 1; // always 1 for your game

    public void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // TEMP: disable the player
        gameObject.SetActive(false);

        // later we will add GameOver events here
        Debug.Log("PLAYER DIED");
    }
}
