using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GeneratedWeapon
{
    /// <summary>
    /// Add the weapon template here, make it parsed in from the gamemanager
    /// </summary>
    public WeaponTemplate template;
    public float damageModifier;
    public int reserveAmmunition;
    public int ammunitionInMagazine;

    public GeneratedWeapon(WeaponTier _tier, WeaponTemplate _template)
    {
        damageModifier = Random.Range(_tier.damageModifier.x, _tier.damageModifier.y);
        template = _template;

        reserveAmmunition = template.startingAmmunition;
        ammunitionInMagazine = template.roundsPerMagazine;
    }
}