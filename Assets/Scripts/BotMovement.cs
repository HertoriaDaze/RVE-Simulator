using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BotMovement : MonoBehaviour
{
    public enum MovementState
    {
        Normal,        
        MovingToTarget,
        Stopped      
    }

    [Header("Movement settings")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 180f;
    public float boundaryCheckDistance = 1f;
    public float arrivalDistance = 1f; 

    [Header("Turn settings")]
    public float minTurnAngle = 60f;
    public float maxTurnAngle = 120f;

    private Rigidbody2D rb;
    private Vector2 currentDirection;
    private MovementState currentState = MovementState.Normal;
    private Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentDirection = Random.insideUnitCircle.normalized;
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case MovementState.Normal:
                MoveForward();
                CheckBoundary();
                break;
                
            case MovementState.MovingToTarget:
                MoveToTarget();
                break;
                
            case MovementState.Stopped:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    public void SetMovementState(MovementState newState, Transform target = null)
    {
        currentState = newState;
        this.target = target;
        
        if (newState == MovementState.Normal)
        {
            currentDirection = Random.insideUnitCircle.normalized;
        }
    }

    void MoveForward()
    {
        rb.linearVelocity = currentDirection * moveSpeed;
    }

    void MoveToTarget()
    {
        if (target == null)
        {
            SetMovementState(MovementState.Normal);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        // 到达目标
        if (Vector2.Distance(transform.position, target.position) <= arrivalDistance)
        {
            SetMovementState(MovementState.Stopped);
            GetComponent<WorkController>().ExecuteTask(); // WorkController
        }

        // 更新朝向
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
    }
    


    void CheckBoundary()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            currentDirection, 
            boundaryCheckDistance,
            LayerMask.GetMask("Boundary")
        );

        if (hit.collider != null)
        {
            TurnRandomly();
        }
    }

    void TurnRandomly()
    {
        float turnAngle = Random.Range(minTurnAngle, maxTurnAngle);
        turnAngle *= Random.value > 0.5f ? 1 : -1;
        currentDirection = Quaternion.Euler(0, 0, turnAngle) * currentDirection;
        
        float targetAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        rb.rotation = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, currentDirection * boundaryCheckDistance);
        
        if (currentState == MovementState.MovingToTarget && target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}