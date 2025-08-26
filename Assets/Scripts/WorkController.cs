using UnityEngine;

public class WorkController : MonoBehaviour
{
    [Header("Check settings")]
    public float detectionRange = 3f;
    public float updateInterval = 1f;
    public float valueThreshold = 100f; 
    
    [Header("Value calculation")]
    public float gamma = 0.90f;
    public float alpha = 0.1f;
    
    private float timer = 0f;
    private float globalTimer = 0f;
    private BotInfor botInfo;
    private BotMovement botMovement;
    private Transform currentTarget = null;

    // [Header("Executing settings")]
    // private bool isExecutingTask = false;
    
    
    public enum DecisionStrategy
    {
        RVE_Model,      
        Greed,    
        Nearest   
    }
    
    
    [Header("Decision Strategy")]
    public DecisionStrategy strategy = DecisionStrategy.RVE_Model;

    
    void Start()
    {
        botInfo = GetComponent<BotInfor>();
        botMovement = GetComponent<BotMovement>();

    }

    void Update()
    {
        if (currentTarget != null) return;
    
        timer += Time.deltaTime;
        globalTimer += Time.deltaTime;
    
        if (timer >= updateInterval)
        {
            timer = 0f;
        
 
            switch(strategy)
            {
                case DecisionStrategy.Greed:
                    CheckNearbyTasks_Greed();
                    break;
                
                case DecisionStrategy.Nearest:
                    CheckNearbyTasks_Nearest();
                    break;
                
                default: 
                    CheckNearbyTasks();
                    break;
            }
        }
    }

    void CheckNearbyTasks()
    {
        GameObject[] allTasks = GameObject.FindGameObjectsWithTag("Task");
        TaskInfo bestTask = null;
        float highestV = 0f;
        float bestDistance = 0f;

        foreach (GameObject taskObj in allTasks)
        {
            float distance = Vector2.Distance(transform.position, taskObj.transform.position);
            
            if (distance <= detectionRange)
            {
                TaskInfo task = taskObj.GetComponent<TaskInfo>();
                if (task != null && !task.finished)
                {
                    float V = CalculateTaskValue(task);
                    // PrintTaskDetails(task, V, distance);
                    
           
                    if (V > valueThreshold && V > highestV)
                    {
                        highestV = V;
                        bestTask = task;
                        bestDistance = distance;
                    }
                }
            }
        }

        
        if (bestTask != null && bestTask.isCooldown == false)
        {
            Debug.Log("Bot"+botInfo.botNumber+$"Find good task ID:{bestTask.taskID} V:{highestV:F2} Distance:{bestDistance:F2} Timer:{globalTimer:F2}");
            SetTarget(bestTask.transform);
        }
        else if (allTasks.Length > 0)
        {
            // Debug.Log("Bot"+botInfo.botNumber+": No tasks were found that reached the threshold");
        }
    }
    
    public void SetTarget(Transform target)
    {
        currentTarget = target;
        globalTimer = 0f; // time = 0
        botMovement.SetMovementState(BotMovement.MovementState.MovingToTarget, target);
    }
    
void CheckNearbyTasks_Greed()
{
    GameObject[] allTasks = GameObject.FindGameObjectsWithTag("Task");
    TaskInfo bestTask = null;
    float bestDistance = float.MaxValue;

    foreach (GameObject taskObj in allTasks)
    {
        float distance = Vector2.Distance(transform.position, taskObj.transform.position);
        
        if (distance <= detectionRange)
        {
            TaskInfo task = taskObj.GetComponent<TaskInfo>();
            if (task != null && !task.finished && !task.isCooldown)
            {
                // p×q×0.01
                float p = GetSuccessCoefficient(task.type);
                float successRate = p * (task.successRate * 0.01f)/2;
                
                if (Random.value <= successRate  && distance < bestDistance)
                {
                    bestTask = task;
                    bestDistance = distance;
                }
            }
        }
    }

    if (bestTask != null)
    {
        Debug.Log($"Bot{botInfo.botNumber} accepted task (SuccessRate) ID:{bestTask.taskID}");
        SetTarget(bestTask.transform);
    }
}

// 策略2：发现就做
void CheckNearbyTasks_Nearest()
{
    GameObject[] allTasks = GameObject.FindGameObjectsWithTag("Task");
    TaskInfo closestTask = null;
    float closestDistance = float.MaxValue;

    foreach (GameObject taskObj in allTasks)
    {
        float distance = Vector2.Distance(transform.position, taskObj.transform.position);
        
        if (distance <= detectionRange)
        {
            TaskInfo task = taskObj.GetComponent<TaskInfo>();
            if (task != null && !task.finished && !task.isCooldown && distance < closestDistance)
            {
                closestTask = task;
                closestDistance = distance;
            }
        }
    }

    if (closestTask != null)
    {
        Debug.Log($"Bot{botInfo.botNumber} accepted task (Always) ID:{closestTask.taskID}");
        SetTarget(closestTask.transform);
    }
}


    public void ExecuteTask()
    {
        TaskInfo taskInfo = currentTarget.GetComponent<TaskInfo>();
        float p = GetSuccessCoefficient(taskInfo.type);
        float q = taskInfo.successRate / 100;
        float rate = p * q;

        if (Random.Range(0f, 1f) <= rate)
        {
            ClearTarget();
        }
        else
        {
            taskInfo.StartCooldown();
            // taskInfo.isCooldown = true;
            currentTarget = null;
            timer = 0f;
            globalTimer = 0f;
            botMovement.SetMovementState(BotMovement.MovementState.Normal);
        }
    }
    public void ClearTarget()
    {
        if (currentTarget != null)
        {
            TaskInfo taskInfo = currentTarget.GetComponent<TaskInfo>();
            if (taskInfo != null)
            {

                taskInfo.finished = true;
                
                botInfo.revenue += taskInfo.revenue;
                
                switch (taskInfo.type)
                {
                    case TaskInfo.TaskType.A:
                        botInfo.finishedA++;
                        break;
                    case TaskInfo.TaskType.B:
                        botInfo.finishedB++;
                        break;
                    case TaskInfo.TaskType.C:
                        botInfo.finishedC++;
                        break;
                }
            
                Debug.Log("Bot"+botInfo.botNumber+$" finished task ID:{taskInfo.taskID} type:{taskInfo.type}\n" +
                          $"Revenue:{taskInfo.revenue} Total revenue:{botInfo.revenue}\n" +
                          $"Task count: A:{botInfo.finishedA} B:{botInfo.finishedB} C:{botInfo.finishedC}");
            }
        }
        
        currentTarget = null;
        timer = 0f;
        globalTimer = 0f;
        botMovement.SetMovementState(BotMovement.MovementState.Normal);
    }
    
    float CalculateTaskValue(TaskInfo task)
    {
        float p = GetSuccessCoefficient(task.type);
        float q = task.successRate / 100;
        float R = task.revenue;
        float t = globalTimer;

        float term1 = p * q * R;
        float exponent = -alpha * t;
        float term2 = gamma * Mathf.Exp(exponent) * (1 - p * q) * R;
        float V = term1 - term2;
        
        return V;
    }

    float GetSuccessCoefficient(TaskInfo.TaskType type)
    {
        switch (type)
        {
            case TaskInfo.TaskType.A: return botInfo.successCoffA;
            case TaskInfo.TaskType.B: return botInfo.successCoffB;
            case TaskInfo.TaskType.C: return botInfo.successCoffC;
            default: return 0f;
        }
    }

    // void PrintTaskDetails(TaskInfo task, float V, float distance)
    // {
    //     float p = GetSuccessCoefficient(task.type);
    //     float q = task.successRate / 100;
    //     float R = task.revenue;
    //     float t = globalTimer;
    //     float exponent = -alpha * t;
    //     float term1 = p * q * R;
    //     float term2 = gamma * Mathf.Exp(exponent) * (1 - p * q) * R;
    //
    //     // string debugMessage = $"\n" +
    //     //     $"=========================";
    //     
    //     Debug.Log("Bot"+botInfo.botNumber+$"Task ID:{task.taskID} Type:{task.type} Distance:{distance:F2} V:{V:F2}");
    // }
}