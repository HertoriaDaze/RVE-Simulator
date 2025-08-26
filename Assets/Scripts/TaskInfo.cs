using UnityEngine;

public class TaskInfo : MonoBehaviour
{
    public enum TaskType { A, B, C }

    [Header("Task Properties")]
    public float revenue;
    public float successRate;
    public bool isCooldown = false;
    public bool finished = false;
    public TaskType type;
    public int taskID;
    public float cooldownDuration = 5f; 
    private float cooldownTimer = 0f;   

    [Header("Visualization")]
    public SpriteRenderer spriteRenderer;
    public Color typeAColor = Color.blue;
    public Color typeBColor = Color.green;
    public Color typeCColor = Color.yellow;
    public Color completedColor = Color.gray;
    public Color cooldownColor = new Color(0.5f, 0.5f, 0.5f, 0.7f); // 半透明灰色

    
    private FailUI failUI;
    
    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        failUI = FindObjectOfType<FailUI>();
        UpdateTaskColor();
    }

    void Update()
    {

        if (isCooldown)
        {
            cooldownTimer += Time.deltaTime;
            spriteRenderer.color = cooldownColor;
            
            if (cooldownTimer >= cooldownDuration)
            {
                isCooldown = false;
                cooldownTimer = 0f;
                UpdateTaskColor();
                Debug.Log($"Task ID:{taskID} not in cool down now");
            }
        }
        else if (finished)
        {
            DestroyTask();
        }
    }

    private void UpdateTaskColor()
    {
        if (spriteRenderer == null) return;

        if (finished)
        {
            spriteRenderer.color = completedColor;
        }
        else
        {
            spriteRenderer.color = type switch
            {
                TaskType.A => typeAColor,
                TaskType.B => typeBColor,
                TaskType.C => typeCColor,
                _ => spriteRenderer.color
            };
        }
    }
    
    public void StartCooldown()
    {
        if (!isCooldown && !finished)
        {
            if(failUI != null)
            {
                failUI.AddFailureTime();
            }

            isCooldown = true;
            cooldownTimer = 0f;
            revenue = (int)(revenue * 0.9);
            Debug.Log($"Tak ID:{taskID} in cool down");
        }
    }
    
    private void DestroyTask()
    {
        if(failUI != null)
        {
            failUI.AddCompleteTime();
        }
        Destroy(gameObject);
    }
    
}