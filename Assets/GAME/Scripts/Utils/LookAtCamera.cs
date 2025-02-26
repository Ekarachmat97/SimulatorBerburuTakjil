using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            transform.LookAt(transform.position + mainCamera.transform.forward);
        }
    }
}
