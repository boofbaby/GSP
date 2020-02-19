using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GeneratedWeapon
{
    public float damageModifier = 1.0f;

    public GeneratedWeapon(WeaponTier _tier)
    {
        damageModifier = Random.Range(_tier.damageModifier.x, _tier.damageModifier.y);
    }
}