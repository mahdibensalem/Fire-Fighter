using UnityEngine;
using UnityEngine.UI;
public class SwipeRotate : MonoBehaviour
{
    private Touch touch;
    private Vector2 touchPosition;
    private Quaternion rotationY;
    public float rotateSpeed = 1f;
    float rotY;
    public GameObject water;
    public GameObject aim;
    
    public  Camera mainCamera;
    public float DistanceZ = 5f;
    Vector3 pos;

    Ray ray;
    private void Start()
    {
        mainCamera = Camera.main;
        Debug.Log("tt1");        
    }
    void FixedUpdate()
    {
        
        water.SetActive(true);
        if (Input.touchCount > 0)
        {
            pos = Input.mousePosition;

            ray = mainCamera.ScreenPointToRay(pos);
        }


        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.name);
            
            Debug.DrawRay(water.transform.position, water.transform.forward * 100, Color.black);
            aim.transform.position = hit.point;
            if (hit.collider.TryGetComponent(out Fire fire))
            {
                Debug.Log("aa");
                fire.TryExtinguishing(.3f * Time.deltaTime);
            }
        }
        water.transform.LookAt(hit.point);
        transform.LookAt(aim.transform);

    }
}
