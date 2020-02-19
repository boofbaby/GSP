using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun")]
    public WeaponTemplate template;
    public GameObject gunModel;
    public Transform gunBarrel;
    public GeneratedWeapon instance;
    private float firetimer;

    [Header("Recoil")]
    public RecoilPattern recoilPattern;
    public Transform recoilBeginPos;
    public Transform recoilEndPos;
    public Vector3 currentOffset;
    public Vector3 currentAngleOffset;
    private Vector3 gunOrigin;
    private Vector3 gunAngleOrigin;

    [Header("Projectile")]
    public Rigidbody projectile;
    public float bulletVelocity = 20f;
    public Vector3 bulletScale = new Vector3(1.0f, 1.0f, 1.0f);
    private enum BulletType { Player, Enemy, Neutral };
    [SerializeField]
    private BulletType type;

    private void Start()
    {
        InitializeVariables();
    }
    private void Update()
    {
        TakeInput();

        gunModel.transform.localPosition = gunOrigin + currentOffset;
        gunModel.transform.localEulerAngles = gunAngleOrigin + currentAngleOffset;
    }

    private void FixedUpdate()
    {
        LerpModelPosition();
    }

    private void InitializeVariables()
    {
        gunOrigin = gunModel.transform.localPosition;
        gunAngleOrigin = gunModel.transform.localEulerAngles;
        currentAngleOffset = Vector3.zero;
        instance = GameManager.Instance.NewWeapon();
    }

    private void TakeInput()
    {
        // DEBUG
        if (Input.GetKeyDown(KeyCode.R))
        {
            instance = GameManager.Instance.NewWeapon();
        }
        // DEBUG END

        if ((Time.time - firetimer > 1 / template.fireRate))
        {
            switch (template.fireMode)
            {
                case WeaponTemplate.WeaponFireMode.Auto:
                        if (Input.GetButton("Fire1")) Shoot();
                    break;
                case WeaponTemplate.WeaponFireMode.Semi:
                        if (Input.GetButtonDown("Fire1")) Shoot();
                    break;
                case WeaponTemplate.WeaponFireMode.Burst:
                    if (Input.GetButtonDown("Fire1")) StartCoroutine(QueueBullet(template.burstAmount));
                    break;
            }
        }
    }

    private Quaternion RaycastCheck()
    {
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit bulletEnd;
        Vector3 bulletPath;

        if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out bulletEnd, Mathf.Infinity))
        {
            bulletPath = (bulletEnd.point - gunBarrel.position).normalized;

            return Quaternion.FromToRotation(projectile.transform.forward, bulletPath);
        }

        return new Quaternion();
    }

    private void Recoil()
    {

        currentOffset += Vector3.back * recoilPattern.speed;
        currentOffset += Vector3.up * recoilPattern.speed / 3;
        currentAngleOffset += Vector3.left * recoilPattern.speed;
    }

    private void LerpModelPosition()
    {
        currentOffset = Vector3.Lerp(currentOffset, Vector3.zero, 0.1f);
        currentAngleOffset = Vector3.Lerp(currentAngleOffset, Vector3.zero, 0.1f);
    }
    
    private void Shoot()
    {
        firetimer = Time.time;
        Quaternion rotation = RaycastCheck();

        Rigidbody instantiatedProjectile = Instantiate(projectile, gunBarrel.position, rotation);
        instantiatedProjectile.velocity = instantiatedProjectile.transform.forward * bulletVelocity;
        instantiatedProjectile.GetComponent<BulletController>().lifetime = template.bulletLifetime;
        instantiatedProjectile.transform.localScale = bulletScale;

        switch (type)
        {
            case BulletType.Player:
                instantiatedProjectile.gameObject.layer = 11;
                break;
            case BulletType.Enemy:
                instantiatedProjectile.gameObject.layer = 12;
                break;
            case BulletType.Neutral:
                break;
        }

        Recoil();
    }

    public IEnumerator QueueBullet(int _numberOfBullets)
    {
        for (int i = 0; i < _numberOfBullets; i++)
        {
            Shoot();
            yield return new WaitForSecondsRealtime(template.burstSpeed);
        }
        yield return null;
    }
}
