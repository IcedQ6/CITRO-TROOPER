using UnityEngine;
using System.Collections;

public class BossMech3 : MonoBehaviour
{
    public float initialMoveSpeed = 2f;
    public float maxMoveSpeed = 10f;  
    public float acceleration = 2f; 
    public float shootCooldown = 0.2f; 
    public GameObject fireSpot1;
    public GameObject fireSpot2;
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
    private BossMain3 bossMain;
    private Boss3Health bossHealth;

    void Start()
    {
        originalCameraPosition = mainCamera.transform.position;
        StartCoroutine(MoveAndShootRoutine());
        bossMain = GameObject.FindObjectOfType<BossMain3>();
        bossHealth = GameObject.FindObjectOfType<Boss3Health>();
    }

    private IEnumerator MoveAndShootRoutine()
{
    while (true)
    {
        yield return new WaitUntil(() => BossMain.GetComponent<BossMain3>().arrived);
        
        yield return StartCoroutine(MoveToTarget(BossMain.transform.position));
        
        bossHealth.forceField.SetActive(true);
        yield return new WaitForSeconds(2f);

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
        GameObject projectile1 = Instantiate(projectilePrefab, fireSpot1.transform.position, fireSpot1.transform.rotation);
        GameObject projectile2 = Instantiate(projectilePrefab, fireSpot2.transform.position, fireSpot2.transform.rotation); 
        BulletBehavior bullet1 = projectile1.GetComponent<BulletBehavior>();
        BulletBehavior bullet2 = projectile2.GetComponent<BulletBehavior>();
        bullet1.speed = 30f; 
        bullet2.speed = 30f; 
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
