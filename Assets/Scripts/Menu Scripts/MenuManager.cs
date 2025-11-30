using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject gameplayMusicPrefab;  // assign in inspector

    public void PlayGame()
    {
        // Start gameplay music ONLY if not already playing
        if (FindObjectOfType<GameplayMusicController>() == null)
        {
            Instantiate(gameplayMusicPrefab);
        }

        SceneManager.LoadScene("GamePlay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
