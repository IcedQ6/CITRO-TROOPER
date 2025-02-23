using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Boss1Health : MonoBehaviour
{
    public static float health = 10f;
    public float bulletDamage = 5f;
    public float EjectSlamDamage = 50f;
    public Image healthBar; 
    public float maxHealth = 800f;
    public GameObject thinbg;
    private BossMain bossMain;

    private void Start()
    {
        bossMain = GameObject.FindObjectOfType<BossMain>();
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EjectSlam")) {
            TakeDamage(EjectSlamDamage);
        }
        if (other.gameObject.CompareTag("Projectile") && !bossMain.invincible) {
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
