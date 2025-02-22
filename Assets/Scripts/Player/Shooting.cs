using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
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
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}
