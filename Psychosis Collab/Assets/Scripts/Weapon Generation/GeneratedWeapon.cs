using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GeneratedWeapon
{
    public WeaponTemplate template;
    public float damageModifier;
    public int reserveAmmunition;
    public int ammunitionInMagazine;
    public float accuracy;

    public GeneratedWeapon(WeaponTier _tier, WeaponTemplate _template)
    {
        damageModifier = Random.Range(_tier.damageModifier.x, _tier.damageModifier.y);
        template = _template;

        accuracy = Random.Range(0.5f, 1f);

        reserveAmmunition = template.startingAmmunition;
        ammunitionInMagazine = template.roundsPerMagazine;
    }

    public GeneratedWeapon(GeneratedWeapon _old)
    {
        damageModifier = _old.damageModifier;
        template = _old.template;

        accuracy = _old.accuracy;

        reserveAmmunition = _old.reserveAmmunition;
        ammunitionInMagazine = _old.ammunitionInMagazine;
    }

    public void Replace(GeneratedWeapon _old)
    {
        damageModifier = _old.damageModifier;
        template = _old.template;

        accuracy = _old.accuracy;

        reserveAmmunition = _old.reserveAmmunition;
        ammunitionInMagazine = _old.ammunitionInMagazine;
    }
}