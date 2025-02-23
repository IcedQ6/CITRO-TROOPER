using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 30f;
    public float bulletDamage = 5f;
    public float EjectSlamDamage = 50f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EjectSlam")) {
            TakeDamage(EjectSlamDamage);
        }
        if (other.gameObject.CompareTag("Projectile")) {
            TakeDamage(bulletDamage);
        }
    }

    void TakeDamage(float amount)
    {
        health -= amount;

        Debug.Log("Enemy Health: " + health);

        if (health <= 0) {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}
