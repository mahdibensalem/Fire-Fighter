using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hose : MonoBehaviour
{
    public Transform hand;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = hand.position;
    }
}
