using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 1;

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
        // Stop timer
        GameTimer timer = FindObjectOfType<GameTimer>();
        if (timer != null)
            timer.StopTimer();

        // Hide score UI
        GameObject scoreUI = GameObject.Find("ScoreText");
        if (scoreUI != null)
            scoreUI.SetActive(false);

        // Hide timer UI
        GameObject timerUIObj = GameObject.Find("TimerText");
        if (timerUIObj != null)
            timerUIObj.SetActive(false);

        // Disable player
        gameObject.SetActive(false);

        // Show Game Over screen
        GameOverManager go = FindObjectOfType<GameOverManager>();
        if (go != null)
            go.ShowGameOver();

        Debug.Log("PLAYER DIED");
    }
}
