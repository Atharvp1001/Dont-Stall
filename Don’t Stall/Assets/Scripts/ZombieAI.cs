using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Wandering Settings")]
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float wanderSpeed = 1.5f;

    [Header("Chasing Settings")]
    [SerializeField] private float chaseSpeed = 3f;

    [Header("Loot Drop Settings")]
    [SerializeField] private GameObject lootPrefab;
    [SerializeField, Range(0f, 1f)] 
    
    private float lootDropChance = 1f / 70f;
    private Vector2 wanderTarget;
    private Vector2 startPosition;
    private CarStateMachine carStateMachine;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;

        carStateMachine = Object.FindFirstObjectByType<CarStateMachine>();
        PickNewWanderTarget();
    }

    void FixedUpdate()
    {
        if (carStateMachine == null || player == null)
            return;

        if (carStateMachine.currentState == CarStateMachine.CarState.Moving)
        {
            Wander();
        }
        else if (carStateMachine.currentState == CarStateMachine.CarState.Stable)
        {
            ChasePlayer();
        }
    }

    private void Wander()
    {
        Vector2 direction = (wanderTarget - rb.position).normalized;
        Vector2 movement = direction * wanderSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        if (Vector2.Distance(rb.position, wanderTarget) < 0.2f)
        {
            PickNewWanderTarget();
        }
    }

    private void PickNewWanderTarget()
    {
        Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
        wanderTarget = startPosition + randomDirection;
    }

    private void ChasePlayer()
    {
        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        Vector2 movement = direction * chaseSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ScoreManager.Instance.IncreaseScore(1);
            TryDropLoot();
            Destroy(gameObject);
        }
    }
    private void TryDropLoot()
    {
        if (lootPrefab == null) return; //If loot Prefab is not set

        if (Random.value < lootDropChance)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
            Debug.Log("Loot dropped!");
        }
    }
}
