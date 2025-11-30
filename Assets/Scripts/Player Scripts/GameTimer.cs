using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public bool isRunning = true;

    private float elapsedTime = 0f;

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetFinalTime()
    {
        return elapsedTime;
    }
}
