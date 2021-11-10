using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{

    private Rigidbody bulletRB;
    public float speed;
    public GameObject bulletHitEffect;

    private void Awake()
    {
        bulletRB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        bulletRB.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            GameObject ding = Instantiate(bulletHitEffect, transform.position, Quaternion.LookRotation(transform.forward, Vector3.up));
            Destroy(ding, 5);
            Destroy(gameObject);
        }
        //Destroy(gameObject);
    }
}
