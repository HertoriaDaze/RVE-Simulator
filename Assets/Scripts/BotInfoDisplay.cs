using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System.Collections.Generic;

public class BotStatsUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text statsText; 
    
    void Start()
    {
        if (statsText == null)
            statsText = GetComponent<TMP_Text>();
        
        UpdateStatsDisplay();
    }

    void Update()
    {
        UpdateStatsDisplay();
        // GameObject[] botObjects = GameObject.FindGameObjectsWithTag("Bot");
        // statsText.text = botObjects.Length.ToString();

    }


    
    void UpdateStatsDisplay()
    {
        
        GameObject[] botObjects = GameObject.FindGameObjectsWithTag("Bot");
        
        StringBuilder sb = new StringBuilder();
        
        
        foreach (GameObject bot in botObjects)
        {
            BotInfor botInfo = bot.GetComponent<BotInfor>();
            sb.AppendLine($"<b>Bot {botInfo.botNumber}:</b>");
            sb.AppendLine($"B: Completed: {botInfo.finishedA} Success coefficient: {botInfo.successCoffA:F2}");
            sb.AppendLine($"G: Completed: {botInfo.finishedB} Success coefficient: {botInfo.successCoffB:F2}");
            sb.AppendLine($"Y: Completed: {botInfo.finishedC} Success coefficient: {botInfo.successCoffC:F2}");
            sb.AppendLine($"Total revenue: {botInfo.revenue:F2}");
            sb.AppendLine();
        }
        
        statsText.text = sb.ToString();
    }


}