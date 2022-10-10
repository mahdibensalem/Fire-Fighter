using UnityEngine;

public class extinguish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.OnFilldWaterBar();
            Destroy(gameObject);
        }
    }
}
