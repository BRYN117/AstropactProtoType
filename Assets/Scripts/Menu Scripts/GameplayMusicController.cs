using UnityEngine;

public class GameplayMusicController : MonoBehaviour
{
    private static GameplayMusicController instance;

    void Awake()
    {
        // Ensure only ONE instance exists
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StopMusic()
    {
        Destroy(gameObject);
    }
}
