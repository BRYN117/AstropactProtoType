using UnityEngine;
using TMPro;

public class WaveUIManager : MonoBehaviour
{
    public static WaveUIManager instance;

    [Header("UI")]
    public TextMeshProUGUI waveText;

    void Awake()
    {
        instance = this;
    }

    // Updates the text to Wave X
    public void SetWave(int waveNumber)
    {
        waveText.text = "Wave " + waveNumber;
    }

    // Updates UI when entering endless mode
    public void SetEndless()
    {
        waveText.text = "ENDLESS";
    }
}
