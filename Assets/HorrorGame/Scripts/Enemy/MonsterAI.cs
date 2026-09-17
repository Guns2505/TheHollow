using UnityEngine;
using UnityEngine.AI;

public enum MonsterState
{
    Patrol,
    Chase,
    Search
}

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 2.2f;
    public float chaseSpeed = 5f;
    public float viewDistance = 22f;
    public float viewAngle = 110f;
    public float walkHearDistance = 7f;
    public float dashHearDistance = 20f;
    public float attackRange = 2.2f;
    public float attackDamage = 34f;
    public float attackCooldown = 1.4f;
    public float searchDuration = 7f;
    public float eyeHeight = 1.7f;

    NavMeshAgent agent;
    Transform player;
    PlayerController playerController;
    PlayerHealth playerHealth;
    MonsterState state = MonsterState.Patrol;
    Vector3 lastKnownPosition;
    int patrolIndex;
    float searchTimeLeft;
    float attackTimeLeft;

    public MonsterState State => state;
    public bool IsChasing => state == MonsterState.Chase;
    public float DistanceToPlayer => player == null ? 999f : Vector3.Distance(transform.position, player.position);

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerController = FindFirstObjectByType<PlayerController>();

        if (playerController != null)
        {
            player = playerController.transform;
            playerHealth = playerController.GetComponent<PlayerHealth>();
        }

        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        if (GameManager.Instance != null && GameManager.Instance.State != GameState.Playing)
        {
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;
        attackTimeLeft -= Time.deltaTime;

        bool detected = CanSeePlayer() || CanHearPlayer();

        switch (state)
        {
            case MonsterState.Patrol:
                UpdatePatrol(detected);
                break;
            case MonsterState.Chase:
                UpdateChase(detected);
                break;
            case MonsterState.Search:
                UpdateSearch(detected);
                break;
        }
    }

    void UpdatePatrol(bool detected)
    {
        agent.speed = patrolSpeed;

        if (detected)
        {
            state = MonsterState.Chase;
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 1.5f)
            GoToNextPatrolPoint();
    }

    void UpdateChase(bool detected)
    {
        agent.speed = chaseSpeed;

        if (detected)
        {
            lastKnownPosition = player.position;
            agent.SetDestination(lastKnownPosition);
            TryAttack();
            return;
        }

        state = MonsterState.Search;
        searchTimeLeft = searchDuration;
        agent.SetDestination(lastKnownPosition);
    }

    void UpdateSearch(bool detected)
    {
        agent.speed = patrolSpeed * 1.4f;

        if (detected)
        {
            state = MonsterState.Chase;
            return;
        }

        searchTimeLeft -= Time.deltaTime;

        if (!agent.pathPending && agent.remainingDistance < 1.5f)
            agent.SetDestination(RandomPointAround(lastKnownPosition, 6f));

        if (searchTimeLeft <= 0f)
        {
            state = MonsterState.Patrol;
            GoToNextPatrolPoint();
        }
    }

    void TryAttack()
    {
        if (DistanceToPlayer > attackRange || attackTimeLeft > 0f) return;

        attackTimeLeft = attackCooldown;
        playerHealth.TakeDamage(attackDamage);
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        patrolIndex = Random.Range(0, patrolPoints.Length);
        agent.SetDestination(patrolPoints[patrolIndex].position);
    }

    Vector3 RandomPointAround(Vector3 center, float radius)
    {
        Vector3 random = center + new Vector3(Random.Range(-radius, radius), 0f, Random.Range(-radius, radius));

        if (NavMesh.SamplePosition(random, out NavMeshHit hit, radius, NavMesh.AllAreas))
            return hit.position;

        return center;
    }

    bool CanSeePlayer()
    {
        Vector3 eye = transform.position + Vector3.up * eyeHeight + transform.forward * 0.5f;
        Vector3 target = player.position + Vector3.up * 1.2f;
        float distance = Vector3.Distance(eye, target);

        if (distance > viewDistance) return false;

        Vector3 direction = (target - eye).normalized;
        if (Vector3.Angle(transform.forward, direction) > viewAngle * 0.5f) return false;

        if (Physics.Linecast(eye, target, out RaycastHit hit, ~0, QueryTriggerInteraction.Ignore) && hit.collider.GetComponentInParent<PlayerController>() == null)
            return false;

        return true;
    }

    bool CanHearPlayer()
    {
        float distance = DistanceToPlayer;

        if (playerController.IsDashing) return distance < dashHearDistance;
        if (playerController.IsMoving) return distance < walkHearDistance;

        return false;
    }
}
