using UnityEngine;
public class WaterPrefab : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float speed;

    void Update()
    {
        transform.position -= transform.right * speed * Time.deltaTime;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fire"))
        {
            Debug.Log("aaezrazer");
            other.GetComponent<healthFire>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
        else
        if (other.CompareTag("WBL"))
        {//**water ball collider limite**//))
            Debug.Log("aaezrazer");

            gameObject.SetActive(false);
        }
    }
}
