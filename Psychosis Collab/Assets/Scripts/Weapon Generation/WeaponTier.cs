using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Tier", menuName = "Weapon Tier")]
public class WeaponTier : ScriptableObject
{
    public float weight;
    public float weightIncreaseModifier;

    public Vector2 damageModifier = new Vector2(1.0f, 1.0f);
}