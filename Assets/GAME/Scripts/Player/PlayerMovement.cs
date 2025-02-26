using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    [Header("Joystick Reference")]
    public Joystick joystick;

    [Header("Spawn Point")]
    public Transform spawnPoint;

    [Header("Player Model")]
    public GameObject playerModel;
    private CharacterController characterController;
    private Animator playerAnimator;

    private Vector3 velocity;
    private bool isGrounded;
    public Camera mainCamera;

    void Start()
    {
       
        // Initialize components
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();

        // Pindahkan pemain ke spawn point saat game dimulai
        TeleportToSpawnPoint();
    }

    void Update()
    {
         moveSpeed = PlayerManager.Instance.movementSpeed;

        // Check if the player is grounded
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset the velocity when grounded
        }

        // Get joystick input
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        // Calculate movement direction relative to the camera
        Vector3 cameraForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.ProjectOnPlane(mainCamera.transform.right, Vector3.up).normalized;
        Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;

        // Movement
        if (moveDirection.magnitude > 0.1f)
        {
            MovePlayer(moveDirection);
            RotatePlayer(moveDirection);
            UpdateAnimation(true);
        }
        else
        {
            UpdateAnimation(false);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void MovePlayer(Vector3 direction)
    {
        Vector3 move = direction.normalized * moveSpeed * Time.deltaTime;
        characterController.Move(move);
    }

    private void RotatePlayer(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = smoothRotation;
    }

    private void UpdateAnimation(bool isMoving)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isMoving", isMoving);
        }
    }

    // Fungsi untuk memindahkan pemain ke spawn point
    public void TeleportToSpawnPoint()
    {
        if (spawnPoint != null)
        {
            characterController.enabled = false;
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            characterController.enabled = true; 
        }
        else
        {
            Debug.LogWarning("Spawn Point belum diatur di PlayerMovement!");
        }
    }
}
