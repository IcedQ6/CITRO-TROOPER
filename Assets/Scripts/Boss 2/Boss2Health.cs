using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Boss2Health : MonoBehaviour
{
    public static float health = 400f;
    public float bulletDamage = 5f;
    public float EjectSlamDamage = 50f;
    public Image healthBar; 
    public float maxHealth = 400f;
    public GameObject thinbg;
    public GameObject forceField;

    private void Start()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EjectSlam")) {
            TakeDamage(EjectSlamDamage);
            forceField.SetActive(false); 
        }
        if (other.gameObject.CompareTag("Projectile")) {
            TakeDamage(bulletDamage);
        }
    }

    void TakeDamage(float amount)
    {
        health -= amount;

        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }

        if (health <= 0) {
            Die();
        }
    }

    void Die()
    {
        Destroy(thinbg);
    }
}
