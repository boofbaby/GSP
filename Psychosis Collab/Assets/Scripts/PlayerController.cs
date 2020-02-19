using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerData data;
    public Gun gun;
    public float reach;

    void Start()
    {
        PullFromGameManager();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (FindObjectOfType<WeaponPickup>() != null)
            {
                WeaponPickup p = GetNearestPickup();

                if (reach >= Vector3.Distance(p.transform.position, transform.position))
                {
                    GeneratedWeapon backup = new GeneratedWeapon(gun.instance);
                    gun.instance.Replace(p.weapon);
                    p.weapon = backup;
                    p.Recreate();
                    gun.SpawnModel();
                }

            }
        }
    }

    public WeaponPickup GetNearestPickup()
    {
        WeaponPickup[] pickups = FindObjectsOfType<WeaponPickup>();

        int closest = 0;
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < pickups.Length; i++)
        {
            if (Vector3.Distance(transform.position, pickups[i].transform.position) < minDistance)
            {
                closest = i;
                minDistance = Vector3.Distance(transform.position, pickups[i].transform.position);
            }
        }

        return pickups[closest];
    }

    private void PullFromGameManager()
    {
        data = GameManager.Instance.currentPlayerData;
        gun.instance = data.weapon;
    }
}
