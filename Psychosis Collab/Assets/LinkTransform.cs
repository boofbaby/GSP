using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkTransform : MonoBehaviour
{
    public Transform link;
    
    void Update()
    {
        transform.localPosition = link.localPosition;
        transform.localRotation = link.localRotation;
    }
}
