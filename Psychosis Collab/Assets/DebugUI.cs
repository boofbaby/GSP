using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public Text ammoDisplay;

    public PlayerData playerData;
    
    void Update()
    {
        if (playerData.weapon.template != null)
        {
            ammoDisplay.text = playerData.weapon.ammunitionInMagazine.ToString() + " / " + playerData.weapon.reserveAmmunition.ToString();
        }
        else
        {
            playerData = FindObjectOfType<PlayerController>().data;
        }
    }
}
