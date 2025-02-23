using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static int hearts = 3;
    public float invincibilityDuration = 1f;
    private float invincibilityTimer = 0f;
    private bool isInvincible = false;

    public Image[] heartList;

    void Update()
    {
        if (isInvincible) 
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0f)
            {
                isInvincible = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !isInvincible && !gameObject.CompareTag("Default") && !gameObject.CompareTag("EjectSlam")) 
        {
            TakeDamage(10); // temp damage amount
            StartInvincibility(); // i-frames
        }
    }

    void TakeDamage(int amount)
    {
        if (hearts > 0)
        {
            hearts--;
            heartList[hearts].gameObject.SetActive(false);
        }

        if (hearts <= 0) 
        {
            Debug.Log("Game Over");
        }
    }

    // i-frames
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

        float flashTime = 0f;

        while (flashTime < invincibilityDuration)
        {
            flashTime += Time.deltaTime;

            if (flashTime % 0.2f < 0.1f)
            {
                mat.color = Color.red;
            }
            else
            {
                mat.color = originalColor;
            }

            yield return null;
        }

        mat.color = originalColor;
    }
}
