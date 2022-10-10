using UnityEngine;

public class CheckFireZone : MonoBehaviour
{
    public int AllFire ;
    private void Awake()
    {
        AllFire = transform.childCount;
    }
}
