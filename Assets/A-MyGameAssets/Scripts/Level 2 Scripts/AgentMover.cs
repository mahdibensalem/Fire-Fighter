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
    [SerializeField] GameObject confetti;
    [SerializeField] GameObject NullGameObject;
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


    bool canActiveColiders = true;
    [Header("Lvl Complete UI")]
    public TextMeshProUGUI CurrentNumLvl;
    public TextMeshProUGUI NextNumLvl;
    public Image lvlImageField;
    public GameObject winPanel;
    public GameObject losePanel;
    int TargetCount;
    public RectTransform ProgressArrow;
    public TextMeshProUGUI CoinTXT;
    public TextMeshProUGUI FireTxt;
    public int NumAllFire;
    public GameObject nozzle;

    [Header("Time Variables")]
    public TextMeshProUGUI TimerText;
    public float timeValue = 90f;
    public Color fontColor;
    public bool showMillSeconds;
    public bool pauseTime = false;
    float _timevalue;

    bool isWin=true;

  [SerializeField] AudioSource fireGone;
  [SerializeField] AudioSource WinAudio;
  [SerializeField] AudioSource LoseAudio;
    private void Awake()
    {
        Instance = this;
        //Anim = GetComponent<Animator>();
        water.gameObject.SetActive(false);
        CurrentNumLvl.text = (SceneManager.GetActiveScene().buildIndex).ToString();
        NextNumLvl.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
        CoinTXT.text = PlayerPrefs.GetInt("Coin").ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        Agent = GetComponent<NavMeshAgent>();
        Agent.destination = targets[0].position;
        ActiveCollider();
        FireTxt.text =( NumAllFire.ToString());
        GameObject m_skin = SkinManager.EquipedSkin?.gameObject ?? NullGameObject;
        thisSkin = Instantiate(m_skin, transform);
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
        GameObject myhand = thisSkin.GetComponent<myHand>().my_hand;
        nozzle.transform.parent = myhand.transform;
        nozzle.transform.localPosition = Vector3.zero;

        #region For Time 
        TimerText.color = fontColor;
        _timevalue = timeValue;
        timeZero();
        displayTime(timeValue);
        #endregion

    }
    void displayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeeconds = timeToDisplay % 1 * 1000;
        if (showMillSeconds)
            TimerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeeconds);
        else
            TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void timeZero()
    {
        if (showMillSeconds)
            TimerText.text = "00:00:000";
        else
            TimerText.text = "00:00";
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

                        //SR.enabled = true;
                        ///////
                        water.SetActive(true);
                        if (Input.touchCount > 0)
                        {
                            pos = Input.mousePosition;
                            ray = mainCamera.ScreenPointToRay(pos);
                        }



                        //thisSkin.transform.rotation = Quaternion.Slerp(thisSkin.transform.rotation, rotation, Time.deltaTime * speedLerpRotation);

                        Vector3 target = new Vector3(aim.transform.position.x, transform.position.y, aim.transform.position.z);
                        //water.transform.rotation = rotation;
                        transform.LookAt(target);
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (Vector3.Distance(hit.point, transform.position) > 10)
                            {
                                if (hit.collider.TryGetComponent(out Fire fire))
                                {
                                    fire.TryExtinguishing(.3f * Time.deltaTime);
                                }
                                aim.transform.position = hit.point;
                            }
                        }
                        ////////////////
                    }
                    else
                    {
                        if (isWin)
                        {
                            confetti.SetActive(true);
                            pauseTime = true;
                            WinAudio.Play();
                            Anim.SetBool("Win", true);
                            VendCam.SetActive(true);
                                if ((SceneManager.GetActiveScene().buildIndex) >= (PlayerPrefs.GetInt("LVL")))
                                PlayerPrefs.SetInt("LVL", SceneManager.GetActiveScene().buildIndex + 1);
                            StartCoroutine(ActiveWinPanel(2f));
                            isWin = false;
                        }
                    }

                    // Done
                }
            }
        }
        //else rotation = Quaternion.Euler(Vector3.zero);
        Anim.SetFloat("Speed", Agent.velocity.sqrMagnitude / 25);


        ///// Timee
        if (!pauseTime)
        {

            if (timeValue > 0)
            {
                timeValue -= Time.deltaTime;
                displayTime(timeValue);
            }
        }
        if (timeValue <= 0)
        {
            TimerText.text = "00:00";
            timeValue = 0;
            Debug.Log("you lose");
            //StartCoroutine(waitToshowLosePanel(1f));
            VendCam.SetActive(true);
            water.SetActive(false);
            losePanel.SetActive(true);
            Anim.SetBool("Lose", true);
            LoseAudio.Play();
            GetComponent<AgentMover>().enabled = false;
        }

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
    public void UpgradeCoin(int amount)
    {
            int money = PlayerPrefs.GetInt("Coin", (PlayerPrefs.GetInt("Coin"))) + amount;
            PlayerPrefs.SetInt("Coin", money);
            CoinTXT.text = money.ToString();
            updateFire();
        timeValue += 5f;
        fireGone.Play();
    }
    void updateFire()
    {
        NumAllFire--;
        FireTxt.text = (NumAllFire.ToString());
    }
}
