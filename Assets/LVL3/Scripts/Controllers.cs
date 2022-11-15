using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Controllers : MonoBehaviour
{
    GameObject thisSkin;
    [SerializeField] TextMeshProUGUI CurrentNumLvl;
    [SerializeField] TextMeshProUGUI NextNumLvl;
    [SerializeField] GameObject NullGameObject;
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject planeMVT;
    [SerializeField] GameObject spawnWaterBall;
    public Animator anim;
    public RuntimeAnimatorController AnimationClipLVL3;
    Vector2 firstFingerDown, SecondFingerDown;
    float PosX;
    float dis;
    [SerializeField] AudioSource LoseAudio;
    private void Awake()
    {
        GameObject m_skin = SkinManager.EquipedSkin?.gameObject ?? NullGameObject;
        thisSkin = Instantiate(m_skin, transform);
        anim = thisSkin.transform.GetComponent<Animator>();
        anim.runtimeAnimatorController = AnimationClipLVL3;
        CurrentNumLvl.text = (SceneManager.GetActiveScene().buildIndex).ToString();
        NextNumLvl.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
    }
    void FixedUpdate()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                firstFingerDown = Camera.main.ScreenToViewportPoint(touch.position);
                PosX = transform.localPosition.x;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                SecondFingerDown = Camera.main.ScreenToViewportPoint(touch.position);
                dis = (((SecondFingerDown.x - firstFingerDown.x ) *2 ));
                transform.localPosition = new Vector3(PosX + (dis * 4.5f), -0.19f, 0);
            }
            if (touch.phase == TouchPhase.Moved)
            {
                dis = 0;
            }
        }
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -4.5f, 4.5f), -0.19f, 0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            // you lose
            losePanel.SetActive(true);
            anim.SetBool("Lose", true);
            planeMVT.GetComponent<PlaneMvmt>().speed = 0;
            spawnWaterBall.GetComponent<SpawnWaterBalls>().enabled = false;
            LoseAudio.Play();
        }
    }
    
}