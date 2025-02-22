using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public float accel = 1.0f;
    public float deaccel = 1.0f;

    public InputAction playerAction;
    public Rigidbody rb;



    private Vector2 movement;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        playerAction.Enable();
    }

    private void OnDisable()
    {
        playerAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement = playerAction.ReadValue<Vector2>();

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10; // Distance from camera

        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 lookDirection = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(lookDirection.x, lookDirection.z) * Mathf.Rad2Deg;

        Debug.Log(angle);

        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(movement.x * moveSpeed, 0, movement.y * moveSpeed);
    }
}
