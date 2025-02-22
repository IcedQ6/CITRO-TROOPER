using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    public float damageAmount = 25f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile")) {
            TakeDamage(damageAmount);
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
