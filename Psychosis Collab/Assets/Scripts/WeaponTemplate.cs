using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Template", menuName = "Weapon Template")]
public class WeaponTemplate : ScriptableObject
{
    public enum WeaponFireMode { Auto, Semi, Burst };

    [Header("Basic")]
    public WeaponFireMode fireMode;
    public RecoilPattern recoilPattern;
    public float baseDamage;
    public float fireRate;
    public float bulletLifetime;

    [Header("Ammunition")]
    public float reloadTime;
    public int startingAmmunition;
    public int maxAmmunition;
    public int roundsPerMagazine;

    [Header("Burst Only")]
    public int burstAmount;
    public float burstSpeed;
}
