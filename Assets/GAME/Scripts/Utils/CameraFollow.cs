using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target; 

    [Header("Offset Settings")]
    public Vector3 offset = new Vector3(0f, 10f, -10f); 

    [Header("Smooth Settings")]
    public float smoothSpeed = 0.125f;

    private void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target belum di-assign pada script CameraFollow.");
            return;
        }

        // Hitung posisi target dengan offset
        Vector3 desiredPosition = target.position + offset;

        // Lerp untuk membuat pergerakan kamera lebih halus
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update posisi kamera
        transform.position = smoothedPosition;

        // Opsional: Lock rotasi kamera (mengarah ke bawah)
        transform.rotation = Quaternion.Euler(40f, 0f, 0f);
    }
}
