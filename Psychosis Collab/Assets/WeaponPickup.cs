using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public bool randomOnStart;
    public GeneratedWeapon weapon;

    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    public void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        if (randomOnStart)
        {
            Generate(5);
        }

        Recreate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Generate(5);
        }
    }

    public void Generate(int _tier)
    {
        weapon = GameManager.Instance.NewWeapon(_tier);
        Recreate();
    }

    public void Recreate()
    {
        meshRenderer.material = weapon.template.model.GetComponent<MeshRenderer>().sharedMaterial;
        meshFilter.mesh = weapon.template.model.GetComponent<MeshFilter>().sharedMesh;
        transform.position += Vector3.up * 0.1f;
    }
}
