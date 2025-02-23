using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2 spawnPos;
    public Vector2 endPos;

    public bool useMoreThanTwoPoints = false;

    public Vector2[] possitionArray;
    private int possitionArrayIndex = 0;

    public float enemyMoveSpeed = 5.0f;

    private bool isMovingToEnd = true;

    // Start is called before the first frame update
    void Start()
    {
        if (!useMoreThanTwoPoints)
        {
            if (spawnPos == null || endPos == null)
            {
                spawnPos = new Vector2(-10, -10);
                endPos = new Vector2(10, 10);
            }
            transform.position = new Vector3(spawnPos.x, 0f, spawnPos.y);
        }
        else
        {
            transform.position = new Vector3(possitionArray[0].x, 0f, possitionArray[0].y);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (useMoreThanTwoPoints == false)
        {
            movingSpawnToEnd();
        }
        else
        {
            moveBetweenPoints();
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

    private void movingSpawnToEnd()
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

    // Moves from index 0 to 1 then n to n+1 and back to 1.
    // The enemy does not return to 0 in this case.
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


}
