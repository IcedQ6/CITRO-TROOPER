using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint1;
    public Transform firePoint2;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float shootCooldown = 0.5f;
    private float lastShotTime = 0f;
    public PlayerController playerController;

    void Start() {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }
    
    void Update()
    {
        if(!playerController.isInMech){
            if(Time.time >= lastShotTime + shootCooldown) {
                Shoot();
                lastShotTime = Time.time;
            }
        }
        if (Input.GetMouseButton(0) && Time.time >= lastShotTime + shootCooldown) {
            Shoot();
            lastShotTime = Time.time;
        }
    }

    void Shoot()
    {
        GameObject bullet1 = Instantiate(bulletPrefab, firePoint1.position, firePoint1.rotation);
        GameObject bullet2 = Instantiate(bulletPrefab, firePoint2.position, firePoint2.rotation);
        Rigidbody rb1 = bullet1.GetComponent<Rigidbody>();
        Rigidbody rb2 = bullet2.GetComponent<Rigidbody>();
        
        rb1.AddForce(firePoint1.forward * bulletForce, ForceMode.Impulse);
        rb2.AddForce(firePoint2.forward * bulletForce, ForceMode.Impulse);

        Destroy(bullet1, 5f);
        Destroy(bullet2, 5f);
    }
}
