using UnityEngine;

public class TaskDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 3f; 
    public LayerMask taskLayer;      
    public string taskTag = "Task";   
    public float checkInterval = 0.5f; 
    private float lastCheckTime;

    void Update()
    {
        if (Time.time - lastCheckTime > checkInterval)
        {
            CheckForTasks();
            lastCheckTime = Time.time;
        }
    }

    void CheckForTasks()
    {

        Collider2D[] taggedTasks = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        foreach (var collider in taggedTasks)
        {
            if (collider.CompareTag(taskTag))
            {
                Debug.Log("YES (Tag): Detected Task " + collider.name);
                return;
            }
        }
        
        Collider2D[] layeredTasks = Physics2D.OverlapCircleAll(
            transform.position, 
            detectionRadius, 
            taskLayer
        );

        if (layeredTasks.Length > 0)
        {
            Debug.Log("YES (Layer): Detected " + layeredTasks.Length + " task(s)");
            return;
        }

        Debug.Log("NO tasks detected");
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}