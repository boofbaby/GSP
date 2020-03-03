using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun")]
    public WeaponModelLink gunModel;
    public Transform modelOrigin;
    public GeneratedWeapon instance;
    private float firetimer;
    public string state;

    [Header("Recoil")]
    public Vector3 currentOffset;
    public Vector3 currentAngleOffset;
    private Vector3 gunOrigin;
    private Vector3 gunAngleOrigin;

    [Header("Projectile")]
    public Vector3 bulletScale = new Vector3(1.0f, 1.0f, 1.0f);
    private enum BulletType { Player, Enemy, Neutral };
    [SerializeField]
    private BulletType type;

    private void Start()
    {
        InitializeVariables();
        SpawnModel();
    }
    private void Update()
    {
        if (gunModel == null) SpawnModel();

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

        state = "normal";
    }

    private void TakeInput()
    {
        // Reload
        if (Input.GetKeyDown(KeyCode.R) && instance.ammunitionInMagazine < instance.template.roundsPerMagazine && instance.reserveAmmunition > 0)
        {
            StartCoroutine(Reload());
        }

        // Shoot
        if ((Time.time - firetimer > 1 / instance.template.fireRate) && (state != "reload"))
        {
            switch (instance.template.fireMode)
            {
                case WeaponTemplate.WeaponFireMode.Auto:
                        if (Input.GetButton("Fire1")) Shoot();
                    break;
                case WeaponTemplate.WeaponFireMode.Semi:
                        if (Input.GetButtonDown("Fire1")) Shoot();
                    break;
                case WeaponTemplate.WeaponFireMode.Burst:
                    if (Input.GetButtonDown("Fire1")) StartCoroutine(QueueBullet(instance.template.burstAmount));
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
            bulletPath = (bulletEnd.point - gunModel.gunBarrel.position).normalized;
            //Debug.Log(bulletEnd.collider.gameObject.tag);
            return Quaternion.FromToRotation(instance.template.projectile.transform.forward, bulletPath);
        }

        return Camera.main.transform.rotation;
    }

    private void Recoil()
    {
        currentOffset += Vector3.back * instance.template.recoilPattern.speed;
        currentOffset += Vector3.up * instance.template.recoilPattern.speed / 3;
        currentAngleOffset += Vector3.left * instance.template.recoilPattern.speed;
    }

    private void LerpModelPosition()
    {
        if (state == "reload")
        {
            currentOffset = Vector3.Lerp(currentOffset, new Vector3(0, -1, 0), 0.001f);
        }
        else
        {
            currentOffset = Vector3.Lerp(currentOffset, Vector3.zero, 0.01f);
            currentAngleOffset = Vector3.Lerp(currentAngleOffset, Vector3.zero, 0.01f);
        }
    }
    
    private void Shoot()
    {
        if ((instance.ammunitionInMagazine > 0) && (state != "reload"))
        {
            firetimer = Time.time;
            Quaternion rotation = RaycastCheck();

            Rigidbody instantiatedProjectile = Instantiate(instance.template.projectile, gunModel.gunBarrel.position, rotation).GetComponent<Rigidbody>();
            instantiatedProjectile.velocity = instantiatedProjectile.transform.forward * instance.template.projectileVelocity;
            instantiatedProjectile.GetComponent<BulletController>().lifetime = instance.template.projectileLifetime;
            instantiatedProjectile.GetComponent<BulletController>().bounces = instance.template.bounces;
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

            instance.ammunitionInMagazine--;
        }

        if (state != "reload" && instance.ammunitionInMagazine <= 0 && instance.reserveAmmunition > 0)
        {
            StartCoroutine(Reload());
        }
    }

    public void SpawnModel()
    {
        StopAllCoroutines();
        if (gunModel != null) Destroy(gunModel.gameObject);
        gunModel = Instantiate(instance.template.model).GetComponent<WeaponModelLink>();
        gunModel.transform.SetParent(modelOrigin);
        state = "normal";
    }

    public IEnumerator QueueBullet(int _numberOfBullets)
    {
        for (int i = 0; i < _numberOfBullets; i++)
        {
            Shoot();
            yield return new WaitForSecondsRealtime(instance.template.burstSpeed);
        }
        yield return null;
    }

    public IEnumerator Reload()
    {
        state = "reload";
        yield return new WaitForSecondsRealtime(instance.template.reloadTime);

        if (instance.reserveAmmunition >= instance.template.roundsPerMagazine)
        {
            instance.reserveAmmunition -= instance.template.roundsPerMagazine - instance.ammunitionInMagazine;
            instance.ammunitionInMagazine = instance.template.roundsPerMagazine;
        }
        else
        {
            if (instance.ammunitionInMagazine + instance.reserveAmmunition <= instance.template.roundsPerMagazine)
            {
                instance.ammunitionInMagazine += instance.reserveAmmunition;
                instance.reserveAmmunition = 0;
            }
            else
            {
                instance.reserveAmmunition = instance.ammunitionInMagazine + instance.reserveAmmunition - instance.template.roundsPerMagazine;
                instance.ammunitionInMagazine = instance.template.roundsPerMagazine;
            }
        }

        state = "normal";
        yield return null;
    }
}
