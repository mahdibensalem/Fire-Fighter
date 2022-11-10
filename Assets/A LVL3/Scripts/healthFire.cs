using TMPro;
using UnityEngine;

public class healthFire : MonoBehaviour
{

    [SerializeField] int health = 100;
    float StartHealth;
    [SerializeField] TextMeshPro healthTXT;
    public GameObject ball, BrokenBall;
    bool ended=true;

    [Header("Particle Effect")]
    float currentIntensity ;
    [SerializeField] private float StartIntensities;
    [SerializeField] private ParticleSystem fireParticleSystems ;
    private void Awake()
    {
        StartHealth = health;
        StartIntensities = fireParticleSystems.emission.rateOverTime.constant;
        
    }
    private void Update()
    {
        
        if (health <= 0 && ended)
        {
            health = 0;
            GetComponent<Collider>().enabled = false;
            Destroy(ball);
            Instantiate(BrokenBall,gameObject.transform);
            Destroy(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(1).gameObject);
            Destroy(gameObject, 2);
            ended = false;
            SpawnFireBall.Instance.SpawnFire();
        }
        healthTXT.text = health.ToString();

    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        ChangeIntensity();
    }
    void ChangeIntensity()
    {

            var emission = fireParticleSystems.emission;
            emission.rateOverTime = ((1/StartHealth)*health)*StartIntensities;
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LF"))
        {
            SpawnFireBall.Instance.SpawnFire();
            Destroy(gameObject);
        }
    }
}
