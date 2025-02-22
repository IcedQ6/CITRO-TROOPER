using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject player;
    public float maxRadius = 10f; 
    private bool wait = true;

    void Update(){
        if(wait == true){
            StartCoroutine(WaitForAttack());
            wait = false;
        } else {
            moveToPlayer();
        }
    }

    void moveToPlayer(){
        Vector3 target = GetRandomPointInRadius();
        var step =  10f * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) < 0.001f) {
            wait = true;
        }
    }

    IEnumerator WaitForAttack()
    {
        yield return new WaitForSeconds(5.0f); 
    }

    public Vector3 GetRandomPointInRadius()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);
        float randomRadius = Random.Range(0f, maxRadius); 

        float xOffset = Mathf.Cos(randomAngle) * randomRadius;
        float yOffset = Mathf.Sin(randomAngle) * randomRadius;

        return player.transform.position + new Vector3(xOffset, yOffset, 0f);
    }
}
