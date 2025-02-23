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
    public float acceleration = 2f;
    public Rigidbody rb;
    public InputAction playerAction;
    private Vector2 movement;

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

            if (playerController.moveToPlayer && !playerController.isInMech && !Input.GetMouseButton(1)) {
                gameObject.tag = "EjectSlam";
                mechTargetPosition = playerController.transform.position;
                
                var step =  mechFlySpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
                if (Vector3.Distance(transform.position, player.transform.position) < 0.001f) { 
                    gameObject.tag = "Player";
                    player.tag = "Default";
                    playerController.isInMech = true;
                    playerController.moveToPlayer = false;
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
}
