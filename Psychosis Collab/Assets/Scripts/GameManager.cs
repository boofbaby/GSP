using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public int floor;
    public WeaponTier[] weaponTiers;
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
    }

    public GeneratedWeapon NewWeapon()
    {
        /// Pick a weapon tier and generate the weapon etc etc lel lel faggot
        return new GeneratedWeapon(weaponTiers[Random.Range(0, weaponTiers.Length)]);
    }
    public GeneratedWeapon NewWeapon(int _mod)
    {
        /// Pick a weapon tier and generate the weapon etc etc lel lel faggot
        return new GeneratedWeapon(weaponTiers[0]);
    }
}
