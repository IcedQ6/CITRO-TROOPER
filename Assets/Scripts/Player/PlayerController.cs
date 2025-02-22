using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Person")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 30.0f;
    public float accel = 1.0f;
    public float deaccel = 1.0f;
    public Collider playerCollider;
    public Rigidbody rb;

    [Header("Sprinting")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float sprintSpeed = 8f;

    [Header("Stamina")]
    public float playerStamina = 100.0f;
    public float maxStamina = 100.0f;
    private bool hasRegenerated = true;
    [Range(0, 50)] public float staminaDrain = 25f;
    [Range(0.0f, 1.0f)] public float staminaRegen = 0.025f;
    public Image staminaProgressUI = null;
    public CanvasGroup sliderCanvasGroup = null;

    [Header("Random")]
    public InputAction playerAction;
    private Vector2 movement;
    private Vector3 rotationTarget;
    private bool moving = false;
    bool flag = false;

    [Header("Mech")]
    public bool isInMech = true;
    public GameObject mech;
    public bool dropDown = false;

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
        sliderCanvasGroup.alpha = 1;
    }

    void Update()
    {
        movement = playerAction.ReadValue<Vector2>();

        if (Input.GetMouseButtonDown(1) && !flag) 
        {
            flag = true;
            if (isInMech) {
                EjectFromMech();
            }
        } 
        else if (Input.GetMouseButtonDown(1) && flag)
        {
            flag = false;
            EnterMech();
        }

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            rotationTarget = hit.point;
        }
        else
        {
            if (mousePos != Vector2.zero) {
                rotationTarget = transform.position + transform.forward * 10f;
            }
        }

        if (isInMech)
        {
            transform.position = mech.transform.position; 
        }
        else
        {
            MovePlayer();
            RotatePlayer();
        }
    }

    private void MovePlayer()
    {
        float currentSpeed = moveSpeed;

        if (movement.x != 0 || movement.y != 0) {
            moving = true;
        } else {
            moving = false;
        }
        
        // sprinting logic
        if (Input.GetKey(sprintKey) && hasRegenerated && moving)
        {
            currentSpeed = sprintSpeed;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0) {
                hasRegenerated = false;
            }
        }
        else // regen stamina
        {
            currentSpeed = moveSpeed;
            if (playerStamina != maxStamina) {
                playerStamina += staminaRegen + Time.deltaTime;
                UpdateStamina(1);

                if (playerStamina >= maxStamina) {
                    playerStamina = maxStamina;
                    hasRegenerated = true;
                }
            }
        }

        rb.velocity = new Vector3(movement.x * currentSpeed, 0, movement.y * currentSpeed);
    }

    private void RotatePlayer()
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

    void EjectFromMech()
    {
        isInMech = false;
    }

    void EnterMech()
    {
        dropDown = true;
    }

    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = playerStamina / maxStamina;

        if (value == 0) {
            sliderCanvasGroup.alpha = 0;
        } else {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
