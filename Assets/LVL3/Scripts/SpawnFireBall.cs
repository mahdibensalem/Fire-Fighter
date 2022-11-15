using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SpawnFireBall : MonoBehaviour
{
    static public SpawnFireBall Instance;

    [SerializeField] private GameObject Confetti, fireBall, WholeIsland, winPanel;
    public float fireDestroyed = 5;
    float _firedestroyed;
    [SerializeField] TextMeshProUGUI coinTxt, FireTxt;
    [SerializeField] GameObject tick;
    [SerializeField] Image LvlProgressImg;
    [SerializeField] int amount;
    [SerializeField] GameObject spawnWater;
    [SerializeField] AudioSource WinAudio;
    [SerializeField] AudioSource FireGoneAudio;
    bool isWin = true;
    void Awake()
    {
        Instance = this;
        coinTxt.text = PlayerPrefs.GetInt("Coin").ToString();

    }
    private void Start()
    {
        UpdateFireTXT();
        _firedestroyed = fireDestroyed;
    }
    public void SpawnFire()
    {
        GameObject go = Instantiate(fireBall, WholeIsland.transform);
        go.transform.position = new Vector3(transform.position.x, 12f, Random.Range(-36f, -41f));
    }
    private void LateUpdate()
    {
        if (fireDestroyed == 0)
        {
            if (isWin)
            {
                // You  Win
                Confetti.SetActive(true);
                WinAudio.Play();
                WholeIsland.GetComponent<PlaneMvmt>().speed = 0;
                GetComponentInParent<Controllers>().anim.SetBool("win", true);
                spawnWater.SetActive(false);
                winPanel.SetActive(true);
                if ((SceneManager.GetActiveScene().buildIndex) >= (PlayerPrefs.GetInt("LVL")))
                    PlayerPrefs.SetInt("LVL", SceneManager.GetActiveScene().buildIndex + 1);
                isWin = false;

            }
        }

    }
    public void UpdateCoins()
    {
        int money = PlayerPrefs.GetInt("Coin", (PlayerPrefs.GetInt("Coin"))) + amount;
        PlayerPrefs.SetInt("Coin", money);
        coinTxt.text = money.ToString();
        FireGoneAudio.Play();
        UpdateFireTXT();
        LVLProgressImg();
    }
    void UpdateFireTXT()
    {
        FireTxt.text = fireDestroyed.ToString();
        if (fireDestroyed == 0)
        {
            tick.SetActive(true);
            FireTxt.text ="";
        }

    }
    void LVLProgressImg()
    {

        LvlProgressImg.fillAmount += 1 / _firedestroyed;
    }
}
