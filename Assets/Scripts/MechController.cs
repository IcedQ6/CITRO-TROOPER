using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MechController : MonoBehaviour
{
    private PlayerController playerController;
    private Vector3 rotationTarget;
    private Vector3 mechTargetPosition;
    public GameObject player;
    public float mechSpeed = 5.0f;
    public float rotationSpeed = 30.0f;
    public float mechFlySpeed = 30f;
    public float acceleration = 2f; // Speed increase per second
    private float currentFlySpeed; // To track the speed increase
    public Rigidbody rb;
    public InputAction playerAction;
    private Vector2 movement;
    public bool moving = false; // so u cant spam right click out of mech

    // Screen Shake Variables
    public Camera mainCamera;         // Reference to the main camera
    public float shakeDuration = 0.2f; // Duration of the shake effect
    public float shakeMagnitude = 0.5f; // Base strength of the shake effect
    private Vector3 originalCameraPosition; // Store original camera position for shake

    private bool isMovingToPlayer = false; // To track if the mech is moving towards the player

    private void OnEnable()
    {
        playerAction.Enable();
    }

    private void OnDisable()
    {
        playerAction.Disable();
    }

    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        originalCameraPosition = mainCamera.transform.position; // Store initial camera position
    }

    void Update()
    {
        if (playerController.isInMech) {
            movement = playerAction.ReadValue<Vector2>();

            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                rotationTarget = hit.point;
            }
            else {
                if (mousePos != Vector2.zero) {
                    rotationTarget = transform.position + transform.forward * 10f;
                }
            }
            
            MoveMech();
            RotateMech();
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
            if(!Input.GetMouseButton(1)){
                moving = true;
            }
            if (playerController.moveToPlayer && !playerController.isInMech && moving) {
                
                if (!isMovingToPlayer) {
                    currentFlySpeed = mechFlySpeed;
                    isMovingToPlayer = true;
                    gameObject.tag = "EjectSlam";
                    mechTargetPosition = playerController.transform.position;
                }

                currentFlySpeed += acceleration * Time.deltaTime;

                var step = currentFlySpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);

                if (Vector3.Distance(transform.position, player.transform.position) < 0.001f) {
                    StartCoroutine(ShakeCamera(currentFlySpeed/200));
                    moving = false;
                    gameObject.tag = "Player";
                    player.tag = "Default";
                    playerController.isInMech = true;
                    playerController.moveToPlayer = false;

                    isMovingToPlayer = false;
                }
            }
        }
    }

    private void MoveMech()
    {
        float currentSpeed = mechSpeed;
        rb.velocity = new Vector3(movement.x * currentSpeed, rb.velocity.y, movement.y * currentSpeed);
    }

    private void RotateMech()
    {
        if (rotationTarget != Vector3.zero) {
            Vector3 directionToLookAt = rotationTarget - transform.position;
            directionToLookAt.y = 0;

            if (directionToLookAt.magnitude > 0.1f) {
                Quaternion targetRotation = Quaternion.LookRotation(directionToLookAt);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    // Coroutine to handle the camera shake effect based on speed
    private IEnumerator ShakeCamera(float intensity)
    {
        float elapsed = 0f;
        float shakeStrength = Mathf.Clamp(intensity, 0f, 1f); // Adjust shake magnitude based on speed

        while (elapsed < shakeDuration)
        {
            // Apply random offset to camera position based on the intensity
            Vector3 shakeOffset = Random.insideUnitSphere * shakeStrength;
            mainCamera.transform.position = originalCameraPosition + shakeOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Reset camera position after shake
        mainCamera.transform.position = originalCameraPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Trigger Screen Shake based on the mech's current speed
            StartCoroutine(ShakeCamera(currentFlySpeed));
        }
    }
}
