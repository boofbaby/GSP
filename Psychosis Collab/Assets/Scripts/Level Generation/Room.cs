using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int tier;
    public bool[] neighbors;
    public GameObject[] walls;

    private void Start()
    {
    }
    
    private void Update()
    {
        
    }

    public void Activate(int _seed)
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (!neighbors[i]) walls[i].SetActive(true);
            else walls[i].SetActive(false);
        }

        tier = Random.Range(1, 6);
    }
}
