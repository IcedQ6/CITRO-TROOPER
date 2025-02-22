using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int lives = 3;
    public float maxHealth = 100.0f;
    public float playerHealth = 100.0f;
    public float invincibilityDuration = 1f;

    private float invincibilityTimer = 0f;
    private bool isInvincible = false;

    public Image healthProgressUI = null;
    public CanvasGroup healthCanvasGroup = null;

    void Start()
    {
        healthCanvasGroup.alpha = 1;
    } 

    void Update()
    {
        if (isInvincible) {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f){
                isInvincible = false;
            }
        }

        if (playerHealth <= 0) {
            lives--;
            playerHealth = 100;

            if (lives <= 0) {
                Debug.Log("Game Over");
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible) {
            TakeDamage(10); // temp damg amt
            StartInvincibility(); // i frames
        }
    }

    void TakeDamage(int amount)
    {
        playerHealth -= amount;
        healthProgressUI.fillAmount = playerHealth / maxHealth;
        Debug.Log("Health: " + playerHealth);
    }

    // i frames
    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = invincibilityDuration; 

        StartCoroutine(FlashPlayer());
    }

    private IEnumerator FlashPlayer()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        Color originalColor = mat.color;

        for (float i = 0; i < invincibilityDuration; i += Time.deltaTime) 
        {
            if (i % 0.2f < 0.1f) {
                mat.color = Color.red;
            } else {
                mat.color = originalColor;
            }

            yield return null;
        }

        mat.color = originalColor;
    }
}
