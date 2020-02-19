using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public int floor;
    public WeaponTier[] weaponTiers;
    public WeaponTemplate[] weaponTemplates;
    public GeneratedWeapon example;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        LoadResources();

        example = NewWeapon();
    }

    private void LoadResources()
    {
        weaponTiers = Resources.LoadAll<WeaponTier>("Weapon Tiers");
        weaponTemplates = Resources.LoadAll<WeaponTemplate>("Weapon Templates");
    }

    public GeneratedWeapon NewWeapon()
    {
        // Pick a weapon tier and generate the weapon etc
        return new GeneratedWeapon(weaponTiers[Random.Range(0,weaponTiers.Length)], weaponTemplates[Random.Range(0, weaponTemplates.Length)]);
    }

    public GeneratedWeapon NewWeapon(int _mod)
    {
        // Pick a weapon tier and generate the weapon etc
        return new GeneratedWeapon(weaponTiers[Random.Range(0, weaponTiers.Length)], weaponTemplates[Random.Range(0, weaponTemplates.Length)]);
    }
}
