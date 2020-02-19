using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject impact;
    public float lifetime;

    private float spawnTime;
    
    void Start()
    {
        spawnTime = Time.time;
    }
    
    void Update()
    {
        if (Time.time > spawnTime + lifetime) Die();
    }

    private void Die()
    {
        GameObject impactInstance = Instantiate(impact);
        impactInstance.transform.position = transform.position;
        ParticleSystemRenderer pr = (ParticleSystemRenderer)impactInstance.GetComponent<ParticleSystem>().GetComponent<Renderer>();
        pr.material = GetComponent<MeshRenderer>().material;

        Destroy(gameObject);
    }
}
