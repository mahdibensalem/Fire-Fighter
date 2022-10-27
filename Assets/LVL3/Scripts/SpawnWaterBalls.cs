using UnityEngine;

public class SpawnWaterBalls : MonoBehaviour
{
    public GameObject WaterBallGO;
    public float spawnTimer;
    float _spawnTimer;
    void Start()
    {
        _spawnTimer = 0;
    }


    void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= spawnTimer)
        {
            SpawnBalls();
        }
    }
    void SpawnBalls()
    {
        _spawnTimer = 0;
        GameObject Waterball = ObjectPool.Instance.GetPooledObject();
        if (Waterball != null)
        {
            Waterball.transform.position = transform.position;
            Waterball.SetActive(true);
        }
    }

}
