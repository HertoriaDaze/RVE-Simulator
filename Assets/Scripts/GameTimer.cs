using UnityEngine;
using TMPro;
public class GameTimer : MonoBehaviour
{
    [Header("Time settings")]
    public float totalTime = 60f; 
    private float currentTime;
    private bool isPaused = false;

    public TMP_Text statsText; 
    
    void Start()
    {
        currentTime = totalTime;
        
        UpdateStatsDisplay();
    }

    void Update()
    {
        if (isPaused) return;


        currentTime -= Time.deltaTime;
        UpdateStatsDisplay();

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            PauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // pause
        
    }
    
    void UpdateStatsDisplay()
    {
        statsText.text = "Countdown: " + (int)(currentTime+1);
    }


}