using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 30f;
    public float lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
}
