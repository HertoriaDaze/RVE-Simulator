using UnityEngine;
using System.Text;
using TMPro;
public class FailUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text statsText; 
    public float failtime = 0;
    public float completetime = 0;
    
    
    // private float countdown = 60f; 
    // private float logInterval = 5f; 
    // private float nextLogTime;
    // private StringBuilder progressLog = new StringBuilder();
    
    private TotalRevenueUI _totalRevenueUI;
    void Start()
    {
        if (statsText == null)
            statsText = GetComponent<TMP_Text>();
        
        // nextLogTime = countdown;
        // _totalRevenueUI = FindObjectOfType<TotalRevenueUI>();
        UpdateStatsDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatsDisplay();
        
        
        // countdown -= Time.deltaTime;
        //
        // // 每5秒添加一段记录
        // if (countdown <= nextLogTime && countdown > 0)
        // {
        //     AddLogEntry();
        //     nextLogTime -= logInterval;
        // }
        //
        // // 倒计时结束打印完整日志
        // if (countdown <= 0)
        // {
        //
        //     Debug.Log(progressLog.ToString());
        //     enabled = false;
        // }
    }
    
    // void AddLogEntry()
    // {
    //     // string countdowntime = (65 - Mathf.CeilToInt(countdown)).ToString();
    //     
    //     // GameObject[] botObjects = GameObject.FindGameObjectsWithTag("Bot");
    //     //
    //     // StringBuilder sb = new StringBuilder();
    //     
    //     // progressLog.Append($"Time:{countdowntime}  Revenue:{_totalRevenueUI.totalRevenue} Fail:{failtime} Complete:{completetime}");
    //     progressLog.Append($"{(int)_totalRevenueUI.totalRevenue};");
    //     // foreach (GameObject bot in botObjects)
    //     // {
    //     //     BotInfor botInfo = bot.GetComponent<BotInfor>();
    //     //     sb.AppendLine($"<b>Bot {botInfo.botNumber}:</b>");
    //     //     sb.AppendLine($"B: Completed: {botInfo.finishedA} Success coefficient: {botInfo.successCoffA:F2}");
    //     //     sb.AppendLine($"G: Completed: {botInfo.finishedB} Success coefficient: {botInfo.successCoffB:F2}");
    //     //     sb.AppendLine($"Y: Completed: {botInfo.finishedC} Success coefficient: {botInfo.successCoffC:F2}");
    //     //     sb.AppendLine($"Total revenue: {botInfo.revenue:F2}");
    //     //     sb.AppendLine();
    //     // }
    //     
    // }

    public void AddFailureTime()
    {
        failtime ++;
    }
    
    public void AddCompleteTime()
    {
        completetime ++;
    }


    void UpdateStatsDisplay()
    {

        statsText.text = 
            ($"Number of failures:{failtime}\n"+ 
             $"Number of complete:{completetime}")
            ;
    }
    
    
    
    
    
}