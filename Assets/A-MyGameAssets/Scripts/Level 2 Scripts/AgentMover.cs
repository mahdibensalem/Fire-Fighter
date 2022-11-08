using Cinemachine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AgentMover : MonoBehaviour
{
    Ray ray;
    Vector3 pos;
    public Camera mainCamera;
    public float speedLerpRotation;
    public static AgentMover Instance;
    public NavMeshAgent Agent;
    public Animator Anim;
    public SwipeRotate SR;

    public List<Transform> targets = new List<Transform>();
    public List<Transform> FireZone = new List<Transform>();
    public List<GameObject> DoorAnim = new List<GameObject>();
    public GameObject VCam, VendCam;

    public CinemachineVirtualCamera virtualcamera;
    [Header("Water")]
    public GameObject water;
    public GameObject aim;
    GameObject thisSkin;
    public RuntimeAnimatorController anim1;
    public Avatar avatar;
    //Transform m_Target;
    Quaternion rotation;
    [Header("Water Bar")]
    public Image waterBar;
    public float speedLoseWater;
    bool canActiveColiders = true;
    [Header("Lvl Complete UI")]
    public TextMeshProUGUI CurrentNumLvl;
    public TextMeshProUGUI NextNumLvl;
    public Image lvlImageField;
    public GameObject winPanel;
    int TargetCount;
    public RectTransform ProgressArrow;
    private void Awake()
    {
        Instance = this;
        //Anim = GetComponent<Animator>();
        water.gameObject.SetActive(false);
        CurrentNumLvl.text = (SceneManager.GetActiveScene().buildIndex).ToString();
        NextNumLvl.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Agent = GetComponent<NavMeshAgent>();
        Agent.destination = targets[0].position;
        ActiveCollider();
        Debug.Log(SkinManager.EquipedSkin.gameObject.name + ":name");
        thisSkin = Instantiate(SkinManager.EquipedSkin.gameObject, transform);
        if (!thisSkin.GetComponent<Animator>())
        {
            Anim = thisSkin.AddComponent<Animator>();
            Anim.avatar = avatar;
        }

        else Anim = thisSkin.GetComponent<Animator>();
        Anim.runtimeAnimatorController = anim1;
        virtualcamera.LookAt = thisSkin.transform;
        virtualcamera.Follow = thisSkin.transform;
        TargetCount = targets.Count;
    }
    void ActiveCollider()
    {

        if (FireZone == null) return;
        Collider[] colliders = FireZone[0].GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(1f / 5f);
        RaycastHit hit;
        // Check if we've reached the destination
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
                {
                    if (targets.Count != 0)
                    {
                        if (canActiveColiders)
                        {

                            ActiveCollider();
                            pos = transform.position;
                            ray.direction =Vector3.forward;
                            Debug.Log(targets[0].GetChild(0).name);
                            targets[0].GetChild(0).gameObject.SetActive(true);
                            VCam.SetActive(false);
                            canActiveColiders = false;
                        }
                        waterBar.fillAmount -= speedLoseWater * Time.deltaTime;
                        //SR.enabled = true;
                        ///////
                        water.SetActive(true);
                        if (Input.touchCount > 0)
                        {
                            pos = Input.mousePosition;
                            ray = mainCamera.ScreenPointToRay(pos);
                        }


                        rotation = Quaternion.LookRotation(aim.transform.position - water.transform.position);
                        //thisSkin.transform.rotation = Quaternion.Slerp(thisSkin.transform.rotation, rotation, Time.deltaTime * speedLerpRotation);

                        Vector3 target = new Vector3(aim.transform.position.x, transform.position.y, aim.transform.position.z);
                        water.transform.rotation = rotation;
                        transform.LookAt(target);
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (Vector3.Distance(hit.point, transform.position) > 10)
                            {
                                Debug.DrawRay(water.transform.position, water.transform.forward * 100, Color.black);
                                if (hit.collider.TryGetComponent(out Fire fire))
                                {
                                    //transform.LookAt(aim.transform);

                                    fire.TryExtinguishing(.3f * Time.deltaTime);
                                }
                                aim.transform.position = hit.point;
                            }
                        }
                        ////////////////
                    }
                    else
                    {
                        Anim.SetBool("Win", true);
                        VendCam.SetActive(true);

                        if ((SceneManager.GetActiveScene().buildIndex) >= (PlayerPrefs.GetInt("LVL")))
                            PlayerPrefs.SetInt("LVL", SceneManager.GetActiveScene().buildIndex + 1);
                        StartCoroutine(ActiveWinPanel(2f));
                    }

                    // Done
                }
            }
        }
        //else rotation = Quaternion.Euler(Vector3.zero);
        Anim.SetFloat("Speed", Agent.velocity.sqrMagnitude / 25);

    }
    IEnumerator  ActiveWinPanel (float time)
    {
        yield return  new WaitForSeconds(time);
        winPanel.SetActive(true);
    }
    public void NextTarget()
    {
        VCam.SetActive(true);
        water.gameObject.SetActive(false);

        Destroy(targets[0].gameObject,2);
        targets.Remove(targets[0]);
        canActiveColiders = true;

        Destroy(FireZone[0].gameObject);
        FireZone.Remove(FireZone[0]);
        if (targets.Count != 0)
        {
            Agent.destination = targets[0].position;
            DoorAnim[0].GetComponent<Animator>().enabled = true;
            DoorAnim.Remove(DoorAnim[0]);

        }
        lvlImageField.fillAmount += 1f/TargetCount;
        ProgressArrow.anchoredPosition = new Vector3((lvlImageField.fillAmount * ((560f) / 1)), 0, 0);
    }
}
