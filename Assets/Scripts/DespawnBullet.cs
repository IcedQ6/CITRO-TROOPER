using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnBullet : MonoBehaviour
{
    float distanceToDespawn = 70f;
    
    void Update()
    {
        if (Mathf.Abs(transform.position.x) > distanceToDespawn || Mathf.Abs(transform.position.z) > distanceToDespawn)
        {
            Destroy(gameObject);
        }
    }
}
