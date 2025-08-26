using UnityEngine;
using System.Collections.Generic;

public class TaskGenerator : MonoBehaviour
{
    [Header("Map settings")]
    public Vector2 mapCenter = Vector2.zero;
    public Vector2 mapSize = new Vector2(30, 30);
    public int seed = 12345;

    [Header("Task settings")]
    public GameObject taskPrefab;
    public int taskCount = 5;
    [Range(0f, 1f)] public float specialTaskChance = 0.2f;
    public float minDistanceBetweenTasks = 1f;

    [Header("Task feature")]
    public Vector2 revenueRange = new Vector2(200, 1000);
    public Vector2 successRateRange = new Vector2(20, 80);
    public float specialTaskRevenue = 2000f;
    public float specialTaskSuccessRate = 20f;

    [Header("Task type distribution")]
    [Range(0f, 1f)] public float typeAChance = 0.4f;
    [Range(0f, 1f)] public float typeBChance = 0.3f;

    [Header("Bot settings")]
    public GameObject botPrefab;
    public int botCount = 3;
    public float botDistanceFromEdge = 1f;
    public float botVerticalPadding = 1f;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private int currentTaskID = 1;
    void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        ClearLevel();
        Random.InitState(seed);

        GenerateBots();
        GenerateTasks();
    }

    void GenerateBots()
    {
        if (botCount <= 0 || botPrefab == null) return;

        
        float usableHeight = mapSize.y - (botVerticalPadding * 2);
        float spacing = usableHeight / (botCount + 1);
        float leftEdge = mapCenter.x - (mapSize.x / 2) + botDistanceFromEdge;

        // generate bot
        float currentY = mapCenter.y - (usableHeight / 2) + spacing;

        for (int i = 0; i < botCount; i++)
        {
            Vector2 spawnPos = new Vector2(leftEdge, currentY);
            GameObject newBot = Instantiate(botPrefab, spawnPos, Quaternion.identity, transform);
            spawnedObjects.Add(newBot);
        
            // bot id
            BotInfor botController = newBot.GetComponent<BotInfor>();
            if (botController != null)
            {
                botController.SetBotNumber(i + 1); 
            }
            else
            {
                Debug.LogWarning("Bot prefab lacks BotController");
            }
        
            currentY += spacing;
        }
    }

    void GenerateTasks()
    {
        if (taskCount <= 0 || taskPrefab == null) return;

        List<Vector2> occupiedPositions = new List<Vector2>();

        for (int i = 0; i < taskCount; i++)
        {
            Vector2 randomPosition = GetRandomTaskPosition(occupiedPositions);
            GameObject newTask = Instantiate(taskPrefab, randomPosition, Quaternion.identity, transform);
            occupiedPositions.Add(randomPosition);
            spawnedObjects.Add(newTask);

            // set task data
            TaskInfo taskData = newTask.GetComponent<TaskInfo>();
            if (taskData != null)
            {
                taskData.taskID = currentTaskID++; 
                SetupTaskData(taskData);
            }
        }
        
    }

    Vector2 GetRandomTaskPosition(List<Vector2> occupiedPositions)
    {
        Vector2 position;
        int maxAttempts = 100;
        int attempts = 0;

        do
        {
            position = new Vector2(
                Random.Range(mapCenter.x - mapSize.x/2, mapCenter.x + mapSize.x/2),
                Random.Range(mapCenter.y - mapSize.y/2, mapCenter.y + mapSize.y/2)
            );
            attempts++;
        }
        while (IsPositionOccupied(position, occupiedPositions) && attempts < maxAttempts);

        return position;
    }

    bool IsPositionOccupied(Vector2 position, List<Vector2> occupiedPositions)
    {
        foreach (Vector2 pos in occupiedPositions)
        {
            if (Vector2.Distance(position, pos) < minDistanceBetweenTasks)
                return true;
        }
        return false;
    }

    void SetupTaskData(TaskInfo taskData)
    {
        if (taskData == null) return;

        // set task type
        float typeRandom = Random.value;
        if (typeRandom < typeAChance)
            taskData.type = TaskInfo.TaskType.A;
        else if (typeRandom < typeAChance + typeBChance)
            taskData.type = TaskInfo.TaskType.B;
        else
            taskData.type = TaskInfo.TaskType.C;

        // special task
        bool isSpecial = Random.value < specialTaskChance;
        if (isSpecial)
        {
            taskData.revenue = specialTaskRevenue;
            taskData.successRate = specialTaskSuccessRate;
 
        }
        else
        {
            float successRate = Random.Range(successRateRange.x, successRateRange.y);
            taskData.revenue = Mathf.Lerp(revenueRange.y, revenueRange.x, 
                Mathf.InverseLerp(successRateRange.x, successRateRange.y, successRate));
            taskData.successRate = successRate;

        }
    }

    void ClearLevel()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null) Destroy(obj);
        }
        spawnedObjects.Clear();
    }

    [ContextMenu("re initial")]
    public void RegenerateLevel()
    {
        GenerateLevel();
    }

    void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(mapCenter, mapSize);
        
        if (botCount > 0)
        {
            float leftEdge = mapCenter.x - (mapSize.x / 2) + botDistanceFromEdge;
            Vector3 lineStart = new Vector3(leftEdge, mapCenter.y - mapSize.y/2 + botVerticalPadding, 0);
            Vector3 lineEnd = new Vector3(leftEdge, mapCenter.y + mapSize.y/2 - botVerticalPadding, 0);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(lineStart, lineEnd);
        }
    }
}