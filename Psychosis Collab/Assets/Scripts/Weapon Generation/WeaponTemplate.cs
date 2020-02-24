using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Template", menuName = "Weapon Template")]
public class WeaponTemplate : ScriptableObject
{
    public enum WeaponFireMode { Auto, Semi, Burst };

    [Header("Basic")]
    public WeaponFireMode fireMode;
    public GameObject model;
    public RecoilPattern recoilPattern;
    public float baseDamage = 3.5f;
    public float fireRate = 3.0f;

    [Header("Projectile")]
    public float projectileVelocity = 10.0f;
    public float projectileLifetime = 1.0f;
    public Vector3 projectileScale = new Vector3(1.0f, 1.0f, 1.0f);
    public GameObject projectile;
    public int bounces = 0;

    [Header("Ammunition")]
    public float reloadTime = 2.0f;
    public int startingAmmunition = 15;
    public int maxAmmunition = 90;
    public int roundsPerMagazine = 15;

    [Header("Burst Only")]
    public int burstAmount = 3;
    public float burstSpeed = 0.2f;
}
