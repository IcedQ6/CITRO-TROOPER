using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Determines enemy AI")]
    [Tooltip("0 is move, 1 is shoot, 2 is charge")]
    public int enemyType = 0;
    private bool doUniqueBehavior = false;
    // For enemy behaviors that only occur for a set amount of time.
    private float uniqueBehvaiorTimer = 0f;

    [Header("Will be overwritten by WaveManager.")]
    public Vector2[] possitionArray;
    private int possitionArrayIndex = 0;
    private int lastPositionIndex = 0;
    public int level;
    public int wave;



    [Header("Adjustable parameters")]
    public float enemyMoveSpeed = 5.0f;


    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(possitionArray[0].x, 0f, possitionArray[0].y);

        
    }

    // Update is called once per frame
    void Update()
    {

        checkForUniqueBehavior();

        if (doUniqueBehavior)
        {
            rotateToPlayer();

            switch (enemyType)
            {
                case 1:
                    stopAndShoot();
                    break;
                case 2:
                    chargeThenReturn();
                    break;
                default:
                    Debug.Log("Invalid call for unique behavior!");
                    break;
            }
        }
        else
        {
            moveBetweenPoints();
        }
            
        
    }

    // Moves from index 0 to 1 then n to n+1 and back to 1.
    // The enemy does not return to 0 in this system.
    private void moveBetweenPoints()
    {
        possitionArrayIndex = distanceToNextIndex();

        if (possitionArrayIndex == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(possitionArray[1].x, 0f, possitionArray[1].y), enemyMoveSpeed * Time.deltaTime);
        }
        else
        {
            int i = possitionArrayIndex + 1;
            if (i == possitionArray.Length) i = 1;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(possitionArray[i].x, 0f, possitionArray[i].y), enemyMoveSpeed * Time.deltaTime);
        }

    }

    // Returns n+1 of the enemy is within .5 units of the next point,
    // which will then cause them to move to the following point.
    private int distanceToNextIndex()
    {
        Vector2 currentPos;
        currentPos = new Vector2(transform.position.x, transform.position.z);

        float distanceToDestination;
        int i = possitionArrayIndex + 1;
        if (i == possitionArray.Length) { i = 1; }
        distanceToDestination = Mathf.Abs(currentPos.x - possitionArray[i].x) + Mathf.Abs(currentPos.y - possitionArray[i].y);

        if (distanceToDestination < 0.5f) return i;

        return possitionArrayIndex;
    }

    // Activates switch to do the unique behavior and sets last index to the current one
    private void checkForUniqueBehavior()
    {
        // Makes sure only types 1 and 2 check for array change
        if (enemyType != 0 && lastPositionIndex != possitionArrayIndex)
        {
            doUniqueBehavior = true;
            uniqueBehvaiorTimer = Time.time;
            lastPositionIndex = possitionArrayIndex;
        }
    }

    private void rotateToPlayer()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (playerPos != Vector3.zero)
        {
            Vector3 directionToLookAt = playerPos - transform.position;
            directionToLookAt.y = 0;

            if (directionToLookAt.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToLookAt);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4.0f * Time.deltaTime);
            }
        }
    }

    [Header("For Enemy Type 1")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shotSpeed = 20f;
    public float shootDelay = 0.5f;
    public float moveDelay = 3.0f;
    private float lastShotTime = 0f;

    private void stopAndShoot()
    {

        if (Time.time >= uniqueBehvaiorTimer + moveDelay)
        {
            doUniqueBehavior = false;
        }

        if (Time.time >= lastShotTime + shootDelay)
        {

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(firePoint.forward * shotSpeed, ForceMode.Impulse);

            lastShotTime = Time.time;
            Debug.Log(lastShotTime);
        }

    }


    [Header("For Enemy Type 2")]
    public float delayTilCharge = 2.0f;
    public float chargeSpeed = 25.0f;
    private bool hitPlayer = false;
    

    private void chargeThenReturn ()
    {
        if (Time.time >= uniqueBehvaiorTimer + delayTilCharge)
        {
            if (!hitPlayer)
            {
                Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
                transform.position = Vector3.MoveTowards(transform.position, playerPos, chargeSpeed * Time.deltaTime);
            }
            else
            {
                int i = possitionArrayIndex;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(possitionArray[i].x, 0f, 
                    possitionArray[i].y), enemyMoveSpeed * Time.deltaTime);

                if (possitionArray[i] == new Vector2(transform.position.x, transform.position.z))
                {
                    hitPlayer = false;
                    doUniqueBehavior = false;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hitPlayer = true;
        }
    }

}
