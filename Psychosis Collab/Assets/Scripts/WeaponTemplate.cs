using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Template", menuName = "Weapon Template")]
public class WeaponTemplate : ScriptableObject
{
    public enum WeaponFireMode { Auto, Semi, Burst };
    public WeaponFireMode fireMode;

    public float baseDamage;
    public float fireRate;
    public float bulletLifetime;

    [Header("Burst Only")]
    public int burstAmount;
    public float burstSpeed;
}
