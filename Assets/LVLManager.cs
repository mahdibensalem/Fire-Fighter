using UnityEngine;
using UnityEngine.SceneManagement;
public class LVLManager : MonoBehaviour
{
    public GameObject[] lvlGO;
    public int _lvlInlocked;
    public GameObject cam;
    public Touch initTouch;
    public float CamPosZ;
    public float speed;
    public Vector3 CamPos;
    [SerializeField] float minPosZ, MaxPosZ;
    public bool canSelect;
    Touch touch;
    private void Awake()
    {
        CamPosZ = cam.transform.position.z;
        CamPos = cam.transform.position;
        int lvlInlocked = PlayerPrefs.GetInt("LVL");
        for (int i=0; i < lvlGO.Length; i++)
        {
            disableLVL(lvlGO[i]);
        }
        for(int j = 0; j < lvlInlocked; j++)
        {
            LVLActive(lvlGO[j]);
        }
        LVLActive(lvlGO[0]);
    }
    void disableLVL(GameObject lvlGO)
    {
        lvlGO.GetComponent<Collider>().enabled = false;
    }
    void LVLActive(GameObject lvlGO)
    {
        lvlGO.GetComponent<Collider>().enabled = true;
        lvlGO.GetComponent<Renderer>().material.color = Color.green;
    }
    private void FixedUpdate()
    {
        if(Input.touchCount>0f)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                initTouch = touch;
                canSelect = true;

            }
            else if (touch.phase == TouchPhase.Moved )
            {
               float DistanceSwipe = initTouch.deltaPosition.y - touch.deltaPosition.y;
                CamPosZ = DistanceSwipe *speed ;
                canSelect = false;
                CamPos = new Vector3(cam.transform.position.x, cam.transform.position.y, CamPosZ + cam.transform.position.z);
                cam.transform.position = CamPos;
            }
            else if(touch.phase == TouchPhase.Ended && canSelect)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("LVL"))
                    {
                        SceneManager.LoadScene(int.Parse(hit.collider.name));
                    }
                }
            }
        }
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, (Mathf.Clamp(cam.transform.position.z, minPosZ, MaxPosZ)));
    }
}
