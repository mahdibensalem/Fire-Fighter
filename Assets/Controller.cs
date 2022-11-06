using UnityEngine;
using UnityEngine.SceneManagement;
public class Controller : MonoBehaviour
{
    public GameObject[] lvlGO;
    [SerializeField] public int _lvlInlocked;
    int lvlInlocked;
    Touch touch;
    [SerializeField] float slideSpeed;
    [SerializeField] Vector3 camPos;
    [SerializeField] bool canSelect=true;
  [SerializeField]  GameObject objectSelected;
    [SerializeField] float MinPos, MaxPos;
    [SerializeField] Material green, Black,yellow;
    Color IniColor;
    private void Awake()
    {
        camPos = transform.position;
        if (PlayerPrefs.GetInt("LVL") != 0)
        {
            lvlInlocked = PlayerPrefs.GetInt("LVL");
        }
        else lvlInlocked = 1;


        for (int i = 0; i < lvlGO.Length; i++)
        {

            disableLVL(lvlGO[i]);
        }
        for (int j = 0; j < lvlInlocked; j++)
        {
            LVLActive(lvlGO[j]);
            IniColor = lvlGO[j].GetComponentInChildren<Renderer>().material.color;
        }
        LVLActive(lvlGO[0]);
        objectSelected=lvlGO[lvlInlocked-1];
        objectSelected.GetComponentInChildren<Renderer>().material = green;
    }
    void disableLVL(GameObject lvlGO)
    {

        lvlGO.GetComponent<Collider>().enabled = false;
        lvlGO.GetComponentInChildren<Renderer>().material = Black;
    }
    void LVLActive(GameObject lvlGO)
    {

        lvlGO.GetComponent<Collider>().enabled = true;
        lvlGO.GetComponentInChildren<Renderer>().material=yellow;
    }
    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Debug.Log("aezr");
                canSelect = true;
                if (Physics.Raycast(ray, out RaycastHit hit))
                {

                    if (!hit.collider.gameObject.CompareTag("LVL"))
                    {
                        canSelect = false;
                        //objectSelected = hit.collider.gameObject;
                        //objectSelected.GetComponentInChildren<Renderer>().material=green;
                    }
                    //else canSelect = false;
                }
            }
            //else
            if (touch.phase == TouchPhase.Moved)
            {
                camPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - (slideSpeed  * touch.deltaPosition.y));
                canSelect = false;
                //if(objectSelected!=null)
                //objectSelected.GetComponentInChildren<Renderer>().material = yellow;
            }
            /*else*/ if (touch.phase == TouchPhase.Ended && canSelect)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("LVL"))
                    {
                        objectSelected = hit.collider.gameObject;
                        SelectLVL();
                    }
                }
            }
        }
        transform.position = Vector3.Lerp(transform.position, camPos, Time.fixedDeltaTime );
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, MinPos, MaxPos));
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(int.Parse(objectSelected.name));
    }
    void SelectLVL()
    {
        for(int i =0;i<lvlInlocked; i++)
        {
            if(lvlGO[i] == objectSelected)
            {
                objectSelected.GetComponentInChildren<Renderer>().material = green;
            }
            else lvlGO[i].GetComponentInChildren<Renderer>().material = yellow;
        }
    
    }
}
