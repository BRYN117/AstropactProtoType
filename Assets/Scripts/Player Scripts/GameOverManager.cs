using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalTimeText;

    private bool isGameOver = false;

    public void ShowGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Freeze game
        Time.timeScale = 0f;

        // Show final score
        finalScoreText.text = ScoreManager.instance.scoreText.text;

        // Show final time
        GameTimer timer = FindObjectOfType<GameTimer>();
        if (timer != null)
            finalTimeText.text = timer.timerText.text;

        gameOverPanel.SetActive(true);
    }

    // Restart gameplay scene — gameplay music continues playing
    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Quit to Main Menu — stop gameplay music
    public void QuitToMenu()
    {
        // Stop the persistent gameplay music
        GameplayMusicController music = FindObjectOfType<GameplayMusicController>();
        if (music != null)
            music.StopMusic();

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
