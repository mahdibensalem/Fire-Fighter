using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    [SerializeField] List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] int amountToPool   ;

    [SerializeField] GameObject BallPrefab;
    [SerializeField] bool willGrow = true;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for(int i=0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(BallPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);

        }
    }
    public GameObject GetPooledObject()
    {
        for (int i =0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        if (willGrow)
        {
            GameObject obj = Instantiate(BallPrefab);
            pooledObjects.Add(obj);
            return obj;
        }
        return null;
    }
}
