using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 5f;
    private Transform[] waypoints;
    private Transform endPoint;
    private int currentWaypointIndex = 0;

    public void SetWaypoints(Transform[] newWaypoints, Transform finalTarget)
    {
        waypoints = newWaypoints;
        endPoint = finalTarget;
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (waypoints != null && currentWaypointIndex < waypoints.Length)
        {
            MoveTowardsTarget(waypoints[currentWaypointIndex].position);

            if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
            {
                currentWaypointIndex++;
                MoveToNextWaypoint();
            }
        }
        else if (endPoint != null) // Jika semua waypoints selesai, lanjut ke endpoint
        {
            MoveTowardsTarget(endPoint.position);

            if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
            {
                Destroy(gameObject); 
            }
        }
    }

    void MoveToNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            RotateTowardsTarget(waypoints[currentWaypointIndex].position);
        }
        else if (endPoint != null)
        {
            RotateTowardsTarget(endPoint.position);
        }
    }

    void MoveTowardsTarget(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    void RotateTowardsTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
        }
    }
}
