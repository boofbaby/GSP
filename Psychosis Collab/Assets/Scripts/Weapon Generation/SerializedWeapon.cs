using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializedWeapon
{
    public int templateIndex;
    public float damageModifier;
    public int reserveAmmunition;
    public int ammunitionInMagazine;

    public SerializedWeapon(GeneratedWeapon _weapon)
    {
        damageModifier = _weapon.damageModifier;
        reserveAmmunition = _weapon.reserveAmmunition;
        ammunitionInMagazine = _weapon.ammunitionInMagazine;

        for (int i = 0; i < GameManager.Instance.weaponTemplates.Length; i++)
        {
            if (GameManager.Instance.weaponTemplates[i] == _weapon.template) templateIndex = i;
        }
    }
}