using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2 spawnPos;
    public Vector2 endPos;
    public float enemyMoveSpeed = 5.0f;

    private bool isMovingToEnd = true;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnPos == null || endPos == null)
        {
            spawnPos = new Vector2(-10, -10);
            endPos = new Vector2(10, 10);
        }
        transform.position = new Vector3(spawnPos.x, 0f, spawnPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkDistanceToDestination() == true) swapMovingToEnd();

        if (isMovingToEnd == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(endPos.x, 0f, endPos.y), enemyMoveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(spawnPos.x, 0f, spawnPos.y), enemyMoveSpeed * Time.deltaTime);
        }
    }

    private bool checkDistanceToDestination()
    {
        Vector2 currentPos;
        currentPos = new Vector2(transform.position.x, transform.position.z);

        float distanceToDestination;
        if (isMovingToEnd == true) distanceToDestination = Mathf.Abs(currentPos.x - endPos.x) + Mathf.Abs(currentPos.y - endPos.y);
        else distanceToDestination = (currentPos.x - spawnPos.x) + (currentPos.y - spawnPos.y);

        if (distanceToDestination < 0.5f) return true;
        else return false;
    }

    private void swapMovingToEnd ()
    {
        isMovingToEnd = !isMovingToEnd;
    }
    
}
