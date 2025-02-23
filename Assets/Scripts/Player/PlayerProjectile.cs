using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public GameObject hitEffectPrefab;
    private void OnTriggerEnter(Collider other)
    {
        GameObject hitEffectObject = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        
        ParticleSystem effect = hitEffectObject.GetComponent<ParticleSystem>();
        if (effect != null)
        {
            effect.Play();
        }

        Destroy(gameObject);
        Destroy(hitEffectObject, effect.main.duration);
    }
}
