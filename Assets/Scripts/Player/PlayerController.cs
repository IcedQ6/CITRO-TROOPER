using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Person")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 30.0f;
    public float accel = 1.0f;
    public float deaccel = 1.0f;
    public Collider playerCollider;
    public Rigidbody rb;

    [Header("Random")]
    public InputAction playerAction;
    private Vector2 movement;
    private Vector3 rotationTarget;

    [Header("Mech")]
    public bool isInMech = true;
    public GameObject mech;
    public bool moveToPlayer = false;

    private void OnEnable()
    {
        playerAction.Enable();
    }

    private void OnDisable()
    {
        playerAction.Disable();
    }

    void Update()
    {
        movement = playerAction.ReadValue<Vector2>();

        if (Input.GetMouseButton(1)) 
        {
            if (isInMech) {
                EjectFromMech();
            }
        } 
        else
        {
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
        rb.velocity = new Vector3(movement.x * moveSpeed, 0, movement.y * moveSpeed);
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
        moveToPlayer = true;
    }
}
