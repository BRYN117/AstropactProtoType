using UnityEngine;

public class PersistentGameplayMusic : MonoBehaviour
{
    private static PersistentGameplayMusic instance;

    void Awake()
    {
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
