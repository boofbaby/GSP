using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score;
    public int floor;
    public PlayerData currentPlayerData;

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
        float totalWeight = 0.0f;
        int chosenIndex = 0;
        float currentWeight = 0.0f;

        for (int i = 0; i < weaponTiers.Length; i++)
        {
            totalWeight += weaponTiers[i].weight * (weaponTiers[i].weightIncreaseModifier * _mod);
        }

        float choice = Random.value * totalWeight;



        float total = 0.0f;

        for (int i = 0; i < weaponTiers.Length; i++)
        {
            total += weaponTiers[i].weight * (weaponTiers[i].weightIncreaseModifier * _mod);

            if (total > choice)
            {
                chosenIndex = i;
                break;
            }
        }

        Debug.Log(weaponTiers[chosenIndex].displayName);

        return new GeneratedWeapon(weaponTiers[chosenIndex], weaponTemplates[Random.Range(0, weaponTemplates.Length)]);
    }
}
