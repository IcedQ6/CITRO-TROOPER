using UnityEngine;
using System.Collections;

public class BossMain : MonoBehaviour
{
    public float moveSpeed = 12.0f;
    public Transform player; 
    public float moveRadius = 10f; 
    private Vector3 targetPosition; 
    public bool arrived = false;

    void Start()
    {
        StartCoroutine(MoveAndShootRoutine());
    }

    private IEnumerator MoveAndShootRoutine()
    {
        while (true)
        {
            targetPosition = GetRandomPositionAroundPlayer();
            yield return StartCoroutine(MoveToTarget(targetPosition));
            yield return new WaitForSeconds(4.0f);
        }
    }

    private Vector3 GetRandomPositionAroundPlayer()
    {
        Vector2 randomDirection = Random.insideUnitCircle * moveRadius;
        Vector3 randomPosition = new Vector3(randomDirection.x, 0f, randomDirection.y);
        randomPosition += player.position; 
        arrived = false;
        return randomPosition;
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f) 
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            Vector3 direction = target - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            }

            yield return null;
        }

        arrived = true;
    }
}
