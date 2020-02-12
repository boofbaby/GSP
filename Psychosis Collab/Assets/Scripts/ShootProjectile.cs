using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public GameObject gun;
    public Rigidbody projectile;
    public float speed = 20f;
    public float size = 1f;
    public float firerate = 10f;
    float firetimer;
    public Transform GunBarrel;
    public Transform recoilBeginPos;
    public Transform recoilEndPos;
    public float recoilSpeed;
    public Vector3 currentOffset;
    public Vector3 currentAngleOffset;
    [SerializeField]
    private Vector3 gunOrigin;
    [SerializeField]
    private Vector3 gunAngleOrigin;
    public Camera camera;
  

    private void Start()
    {
        gunOrigin = gun.transform.localPosition;
        gunAngleOrigin = gun.transform.localEulerAngles;
        currentAngleOffset = Vector3.zero;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (Time.time - firetimer > 1 / firerate)
            {
                firetimer = Time.time;
                RaycastCheck();
                Recoil();

                //Physics.IgnoreCollision(instantiatedProjectile.GetComponent<SphereCollider>(), GetComponent<CapsuleCollider>());
                //Physics.IgnoreCollision(instantiatedProjectile.GetComponent<SphereCollider>(), GunBarrel.GetComponentInParent<BoxCollider>());
   
            }

        }

        gun.transform.localPosition = gunOrigin + currentOffset;
        gun.transform.localEulerAngles = gunAngleOrigin + currentAngleOffset;
    }

    private void FixedUpdate()
    {
        currentOffset = Vector3.Lerp(currentOffset, Vector3.zero, 0.1f);
        currentAngleOffset = Vector3.Lerp(currentAngleOffset, Vector3.zero, 0.1f);
    }


    public void Recoil()
    {
        currentOffset += Vector3.back * recoilSpeed;
        currentOffset += Vector3.up * recoilSpeed / 3;
        currentAngleOffset += Vector3.left * recoilSpeed;
    }

    public void RaycastCheck()
    {
        Vector3 rayOrigin = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit bulletEnd;
        Vector3 bulletPath;

        if (Physics.Raycast(rayOrigin, camera.transform.forward, out bulletEnd, Mathf.Infinity))
        {
            bulletPath = (bulletEnd.point - GunBarrel.position).normalized;

            Quaternion rotation = Quaternion.FromToRotation(projectile.transform.forward, bulletPath);
            print(rotation);
            Rigidbody instantiatedProjectile = Instantiate(projectile, GunBarrel.position, rotation);
            instantiatedProjectile.velocity = instantiatedProjectile.transform.forward * speed;
            instantiatedProjectile.transform.localScale = new Vector3(size, size, size);

        }
        
    }


}



