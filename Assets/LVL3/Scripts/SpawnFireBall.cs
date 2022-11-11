using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireBall : MonoBehaviour
{
    static public SpawnFireBall Instance;
    [SerializeField] private GameObject fireBall,WholeIsland;
    public int fireDestroyed=5;
    void Awake()
    {
        Instance = this;
    }
    public void SpawnFire()
    {
        GameObject go=Instantiate(fireBall, WholeIsland.transform);
        go.transform.localPosition = new Vector3(transform.position.x, 12f, -40f);
    }
    private void LateUpdate()
    {
        if (fireDestroyed == 0)
        {
            // You  Win
            WholeIsland.GetComponent<PlaneMvmt>().speed = 0;

        }

    }

}
