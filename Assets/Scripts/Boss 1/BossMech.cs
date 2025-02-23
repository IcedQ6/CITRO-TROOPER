using UnityEngine;
using System.Collections;

public class BossMech : MonoBehaviour
{
    public float initialMoveSpeed = 2f;
    public float maxMoveSpeed = 10f;  
    public float acceleration = 2f; 
    public float shootCooldown = 0.2f; 
    public GameObject fireSpot;
    public GameObject projectilePrefab;
    public float rotationSpeed = 90f; 
    public float shootingDuration = 2f; 
    public GameObject BossMain;
    private bool isShooting = false; 
    private float lastShootTime = 0f; 
    private float shootingTimeElapsed = 0f; 
    public Camera mainCamera;       
    public float shakeDuration = 0.2f;
    private Vector3 originalCameraPosition; 
    private BossMain bossMain;

    void Start()
    {
        originalCameraPosition = mainCamera.transform.position;
        StartCoroutine(MoveAndShootRoutine());
        bossMain = GameObject.FindObjectOfType<BossMain>();
    }

    private IEnumerator MoveAndShootRoutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => BossMain.GetComponent<BossMain>().arrived);
            yield return StartCoroutine(MoveToTarget(BossMain.transform.position));

            isShooting = true;
            shootingTimeElapsed = 0f;

            yield return StartCoroutine(SpinAndShoot());
        }
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        float currentSpeed = initialMoveSpeed;
        
        while (Vector3.Distance(transform.position, target) > 0.05f) 
        {
            transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.deltaTime);

            Vector3 direction = target - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            }
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxMoveSpeed);

            yield return null;
        }
        bossMain.invincible = true;
        StartCoroutine(ShakeCamera(currentSpeed));
    }

    private IEnumerator SpinAndShoot()
    {
        while (isShooting && shootingTimeElapsed < shootingDuration)
        {
            if (Time.time >= lastShootTime + shootCooldown)
            {
                ShootProjectile();
                lastShootTime = Time.time;
            }

            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            shootingTimeElapsed += Time.deltaTime;

            yield return null;
        }

        isShooting = false;
    }

    private void ShootProjectile()
    {
        if (fireSpot != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, fireSpot.transform.position, fireSpot.transform.rotation);
            
            BulletBehavior bullet = projectile.GetComponent<BulletBehavior>();
            if (bullet != null)
            {
                bullet.speed = 30f; 
            }
        }
    }

    private IEnumerator ShakeCamera(float intensity)
    {
        float elapsed = 0f;
        float shakeStrength = Mathf.Clamp(intensity, 0f, 1f); 

        while (elapsed < shakeDuration)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeStrength;
            mainCamera.transform.position = originalCameraPosition + shakeOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        float returnSpeed = 5f;
        while (Vector3.Distance(mainCamera.transform.position, originalCameraPosition) > 0.01f)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, originalCameraPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }
        mainCamera.transform.position = originalCameraPosition;
    }

}
