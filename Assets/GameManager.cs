using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject LosePanel;
    [Header("Water")]
    public float waterCollect;
    public float ValueWaterCollection;
    public TextMeshProUGUI waterTXT;
    public GameObject waterCheck;
    AudioSource extinguishSound;

    [Header("Lvl Progress UI")]
    [SerializeField]
    public GameObject player;
    public GameObject WinObject;
    public TextMeshProUGUI CurrentNumLvl;
    public TextMeshProUGUI NextNumLvl;
    public RectTransform ProgressArrow;
    public Image lvlImageProgress;

    float distance;


    private void Awake()
    {
        extinguishSound=GetComponent<AudioSource>();
        Instance = this;
    }
    void Start()
    {

        distance = Vector3.Distance(player.transform.position, WinObject.transform.position);
        CurrentNumLvl.text = SceneManager.GetActiveScene().buildIndex.ToString();
        NextNumLvl.text = (SceneManager.GetActiveScene().buildIndex+1).ToString();
        waterTXT.text = waterCollect.ToString();
    }
    private void LateUpdate()
    {
         float m_distance = Vector3.Distance(player.transform.position, WinObject.transform.position);
        lvlImageProgress.fillAmount = 1-(m_distance / distance);
        //ProgressArrow.position =  new Vector3((lvlImageProgress.fillAmount * ((560f)/1))-280f, 0, 0);
        ProgressArrow.anchoredPosition = new Vector3((lvlImageProgress.fillAmount * ((560f)/1)), 0, 0);
    }
    public void UpgradeWaterCollect()
    {
        extinguishSound.Play();

        waterCollect--;
        waterTXT.text = waterCollect.ToString();
        if (waterCollect == 0)
        {
            waterCheck.SetActive(true);
            waterTXT.gameObject.SetActive(false);
        }

        
        //waterBar.fillAmount = waterCollect;
    }
    public void OnLose()
    {
        LosePanel.SetActive(true);
        TinySauce.OnGameFinished(false, lvlImageProgress.fillAmount);


    }
    public void OnWin()
    {

    }
    public void GoToSecondLVL()
    {
        

    }
}
