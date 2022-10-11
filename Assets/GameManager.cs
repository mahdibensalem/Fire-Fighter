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
    public Image waterBar;


    [Header("Lvl Complete UI")]
    [SerializeField]
    public GameObject player;
    public GameObject WinObject;
    public TextMeshProUGUI CurrentNumLvl;
    public TextMeshProUGUI NextNumLvl;
    public Image lvlImageField;
    float distance;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        waterBar.fillAmount = waterCollect;
        distance = Vector3.Distance(player.transform.position, WinObject.transform.position);
        CurrentNumLvl.text = SceneManager.GetActiveScene().buildIndex.ToString();
        NextNumLvl.text = (SceneManager.GetActiveScene().buildIndex+1).ToString();
    }
    private void LateUpdate()
    {
         float m_distance = Vector3.Distance(player.transform.position, WinObject.transform.position);
        lvlImageField.fillAmount = 1-(m_distance / distance);

    }
    public void OnFilldWaterBar()
    {
        waterCollect+= ValueWaterCollection;
        waterBar.fillAmount = waterCollect;
    }
    public void OnLose()
    {
        LosePanel.SetActive(true);

    }
    public void OnWin()
    {

    }
    public void GoToSecondLVL()
    {
        

    }
}
