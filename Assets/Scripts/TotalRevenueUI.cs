using UnityEngine;
using System.Text;
using TMPro;
public class TotalRevenueUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text statsText; 
    public float totalRevenue = 0;
    
    
    private float countdown = 60f; 
    private float logInterval = 5f; 
    private float nextLogTime;
    private StringBuilder progressLog = new StringBuilder();
    
    void Start()
    {
        if (statsText == null)
            statsText = GetComponent<TMP_Text>();
        
        
        nextLogTime = countdown;
        
        UpdateStatsDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
        countdown -= Time.deltaTime;
        
        if (countdown <= nextLogTime && countdown > 0)
        {
            AddLogEntry();
            nextLogTime -= logInterval;
        }
        
        if (countdown <= 0)
        {

            Debug.Log(progressLog.ToString());
            enabled = false;
        }
        
        
        
        UpdateStatsDisplay();
    }
    
    void AddLogEntry()
    {
        progressLog.Append($"{(int)totalRevenue},");
    }


    void UpdateStatsDisplay()
    {
        totalRevenue = 0;
        GameObject[] botObjects = GameObject.FindGameObjectsWithTag("Bot");
        
        foreach (GameObject bot in botObjects)
        {
            BotInfor botInfo = bot.GetComponent<BotInfor>();
            totalRevenue = totalRevenue + botInfo.revenue;

        }
        statsText.text = "Total revenue: "+totalRevenue;
    }
}
