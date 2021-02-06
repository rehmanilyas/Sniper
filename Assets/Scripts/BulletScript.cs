using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float timeToLive = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 20f;
        timeToLive -= Time.deltaTime;
        if (timeToLive < 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        ParticleEffects.Instance.SparksAt(transform.position);

        if (other.attachedRigidbody != null)
            other.attachedRigidbody.AddForceAtPosition(transform.forward * 1000f, transform.position);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ParticleEffects.Instance.SparksAt(collision.contacts[0].point);
        if (collision.collider.attachedRigidbody != null)
            collision.collider.attachedRigidbody.AddForceAtPosition(transform.forward * 100f, collision.contacts[0].point);
        Destroy(gameObject);
    }
}
