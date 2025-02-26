using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SmartNPC : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints;
    private Animator animator;
    public float waitTime = 2f;
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            MoveToNextWaypoint();
        }
    }

    void Update()
    {
        // Set animasi berdasarkan kecepatan NPC
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);

        // Cek apakah NPC sudah sampai atau jalur gagal
        if (!isWaiting && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
            {
                StartCoroutine(WaitAtWaypoint());
            }
            else if (agent.path.status == NavMeshPathStatus.PathInvalid || agent.path.status == NavMeshPathStatus.PathPartial)
            {
                FindNewPath();
            }
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length > 0)
        {
            isWaiting = false;
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            SetSmartDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void SetSmartDestination(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(targetPosition, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(targetPosition);
        }
        else
        {
            FindNewPath();
        }
    }

    void FindNewPath()
    {
        Vector3 randomPosition = GetRandomNavMeshPosition(transform.position, 5f);
        if (randomPosition != Vector3.zero)
        {
            agent.SetDestination(randomPosition);
        }
        else
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    Vector3 GetRandomNavMeshPosition(Vector3 origin, float range)
    {
        for (int i = 0; i < 10; i++) // Coba 10 kali cari posisi yang valid
        {
            Vector3 randomPoint = origin + Random.insideUnitSphere * range;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, range, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return Vector3.zero; // Jika gagal, kembalikan Vector3.zero
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        agent.ResetPath();
        yield return new WaitForSeconds(waitTime);
        MoveToNextWaypoint();
    }
}
