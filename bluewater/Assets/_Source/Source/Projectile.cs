using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void Launch(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction * speed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit: " + collision.gameObject.name);

        Destroy(gameObject);
    }
}