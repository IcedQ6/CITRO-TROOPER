using UnityEngine;
using System.Collections;

public class BossSpawner : MonoBehaviour
{
    public GameObject[] bossObjects;  // Array to hold the pre-existing bosses
    public GameObject[] Titles;  // Array to hold the pre-existing bosses
    public GameObject[] music;  // Array to hold the pre-existing bosses
    public GameObject countdown;
    private int currentBossIndex = 0;  // Track which boss is being spawned next

    public Camera mainCamera;  // Reference to the camera for screen shake
    public float shakeDuration = 0.3f;  // Duration of screen shake
    public float shakeMagnitude = 0.05f;  // Magnitude of screen shake
    public float initialDelay = 5f;  // Time to wait before spawning the first boss
    private title countdowntimer;
    private int count = 0;
    private int mCount = 0;

    private Vector3 originalCameraPos;  // Store the original camera position

    void Start()
    {
        countdowntimer = GameObject.FindObjectOfType<title>();
        if (bossObjects.Length == 3)  // Ensure there are exactly 3 bosses
        {
            originalCameraPos = mainCamera.transform.position;  // Store the original camera position
            StartCoroutine(WaitAndStartSpawning());
        }
        else
        {
            Debug.LogError("You need exactly 3 boss objects in the scene!");
        }
    }

    IEnumerator WaitAndStartSpawning()
    {
        // Wait for the initial 5 seconds before spawning the first boss
        Titles[count].SetActive(true);
        countdown.SetActive(true);
        yield return new WaitForSeconds(initialDelay);
        SpawnNextBoss();
    }

    void SpawnNextBoss()
    {
        if (currentBossIndex < bossObjects.Length)
        {
            music[mCount].SetActive(true);
            // Activate the next boss in the sequence
            GameObject boss = bossObjects[currentBossIndex];
            boss.SetActive(true);

            // Trigger screen shake when the boss spawns
            
            StartCoroutine(ShakeCamera(10));
            Titles[count].SetActive(false);
            countdown.SetActive(false);
            count++;
            // Increment the index for the next boss
            currentBossIndex++;

            // Wait for the boss to be destroyed before spawning the next one
            StartCoroutine(WaitForBossDeath(boss));
        }
    }

    IEnumerator WaitForBossDeath(GameObject boss)
    {
        // Wait until the boss is destroyed
        while (boss != null && boss.activeInHierarchy)
        {
            yield return null;
        }

        // Wait 5 seconds after the boss is destroyed
        Titles[count].SetActive(true);
        countdown.SetActive(true);
        music[mCount].SetActive(false);
        mCount++;
        yield return new WaitForSeconds(5f);

        // Spawn the next boss
        SpawnNextBoss();
    }

    private IEnumerator ShakeCamera(float intensity)
    {
        float elapsed = 0f;
        float shakeStrength = Mathf.Clamp(intensity, 0f, 1f); 

        while (elapsed < shakeDuration)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeStrength;
            mainCamera.transform.position = originalCameraPos + shakeOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalCameraPos;
    }
}
