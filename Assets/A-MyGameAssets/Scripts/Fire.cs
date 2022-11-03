using UnityEngine;
using UnityEngine.UI;
public class Fire : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float currentIntensity = 1.0f;
    [SerializeField] private float[] StartIntensities = new float[0];
    [SerializeField] private ParticleSystem[] fireParticleSystems = new ParticleSystem[0];
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject canvasHealth;
    public GameObject TextPrefab;
    bool canDestroy = true;
    bool isExtinguishing;
    void Start()
    {
        StartIntensities = new float[fireParticleSystems.Length];
        for (int i = 0; i < fireParticleSystems.Length; i++)
        {
            StartIntensities[i] = fireParticleSystems[i].emission.rateOverTime.constant;

        }
    }

    public bool TryExtinguishing(float amount)
    {
        canvasHealth.SetActive(true);
        timeLastWatered = Time.time;
        currentIntensity -= amount;
        ChangeIntensity();
        healthBar.color = Color.green;
        if (currentIntensity <= 0.05f && canDestroy)
        {
            isLit = false;
           GameObject go = Instantiate(TextPrefab, transform.position, Quaternion.identity);
            go.transform.LookAt(Camera.main.transform);
            destroy();
            canDestroy = false;

            return true;

        }
        return false; //fire is out
    }
    float timeLastWatered = 0;
    [SerializeField] float regenDelay = 2.5f;
    [SerializeField] float regenRate = .1f;
    bool isLit = true;

    private void Update()
    {
        if (currentIntensity <= 1.0f && Time.time - timeLastWatered >= regenDelay)
        {
            healthBar.color = Color.red;
            currentIntensity += regenRate * Time.deltaTime;
            ChangeIntensity();
        }
        canvasHealth.transform.LookAt(Camera.main.transform);
    }
    void destroy()
    {
        if (GetComponentInParent<CheckFireZone>().AllFire <= 1)
        {
            AgentMover.Instance.NextTarget();
        }
        GetComponentInParent<CheckFireZone>().AllFire--;
        Destroy(gameObject);

    }
    void ChangeIntensity()
    {
        for (int i = 0; i < fireParticleSystems.Length; i++)
        {
            var emission = fireParticleSystems[i].emission;
            emission.rateOverTime = currentIntensity * StartIntensities[i];
        }
        healthBar.fillAmount = currentIntensity;
    }
}
