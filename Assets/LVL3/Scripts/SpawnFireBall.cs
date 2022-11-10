using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireBall : MonoBehaviour
{
    static public SpawnFireBall Instance;
    [SerializeField] private GameObject fireBall,WholeIsland;
    void Awake()
    {
        Instance = this;
    }
    public void SpawnFire()
    {
        GameObject go=Instantiate(fireBall, WholeIsland.transform);
        go.transform.localPosition = new Vector3(transform.position.x, 12f, -40f);
    }

}
