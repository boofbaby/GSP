using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{

    public Rigidbody projectile;
    public float speed = 20f;
    public float size = 1f;
    public float firerate = 10f;
    float firetimer;
    public GameObject GunBarrel;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("Fire1"))
        {
            if (Time.time - firetimer > 1 / firerate)
            {
                firetimer = Time.time;
                Rigidbody instantiatedProjectile = Instantiate(projectile, GunBarrel.transform.position, transform.rotation);

                instantiatedProjectile.velocity = Camera.main.transform.forward * speed;
                instantiatedProjectile.transform.localScale = new Vector3(size, size, size);

                Physics.IgnoreCollision(instantiatedProjectile.GetComponent<SphereCollider>(), GetComponent<CapsuleCollider>());
                Physics.IgnoreCollision(instantiatedProjectile.GetComponent<SphereCollider>(), GunBarrel.GetComponentInParent<BoxCollider>());
            }

        }

    }
}

