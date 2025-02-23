using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossScript : MonoBehaviour
{
    public GameObject player;
    public float maxRadius = 10f;
    private bool isMoving = false;
    private Vector3 targetPosition;

    void Update()
    {
        if (!isMoving)
        {
            StartCoroutine(WaitAndPickNewTarget());
        }
        else
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        var step = 12f * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false; 
        }
    }

    IEnumerator WaitAndPickNewTarget()
    {
        yield return new WaitForSeconds(3.0f);
        targetPosition = GetRandomPointBehindPlayer();
        isMoving = true;
    }

    public Vector3 GetRandomPointBehindPlayer()
    {
        float randomAngle = Random.Range(Mathf.PI, Mathf.PI * 2f); 
        float randomRadius = Random.Range(0f, maxRadius);

        float xOffset = Mathf.Cos(randomAngle) * randomRadius;
        float yOffset = Mathf.Sin(randomAngle) * randomRadius;

        return player.transform.position + new Vector3(xOffset, yOffset, 0f);
    }
}
