using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SpawnFireBall : MonoBehaviour
{
    static public SpawnFireBall Instance;
    [SerializeField] private GameObject fireBall,WholeIsland,winPanel;
    public float fireDestroyed=5;
    float _firedestroyed;
    [SerializeField] TextMeshProUGUI coinTxt,FireTxt;
    [SerializeField] Image LvlProgressImg;
    [SerializeField] int amount;
    [SerializeField] GameObject spawnWater;
    [SerializeField] AudioSource WinAudio;
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
        GameObject go=Instantiate(fireBall, WholeIsland.transform);
        go.transform.position = new Vector3(transform.position.x, 12f,Random.Range(-36f,-41f));
    }
    private void LateUpdate()
    {
        if (fireDestroyed == 0)
        {
            // You  Win
            WinAudio.Play();
            WholeIsland.GetComponent<PlaneMvmt>().speed = 0;
            GetComponentInParent<Controllers>().anim.SetTrigger("Win");
            spawnWater.SetActive(false);
            winPanel.SetActive(true);
        }

    }
    public void UpdateCoins()
    {
        int money = PlayerPrefs.GetInt("Coin", (PlayerPrefs.GetInt("Coin"))) + amount;
        PlayerPrefs.SetInt("Coin", money);
        coinTxt.text = money.ToString();
        UpdateFireTXT();
        LVLProgressImg();
    }
    void UpdateFireTXT()
    {
        FireTxt.text = fireDestroyed.ToString();
            
    }
    void LVLProgressImg()
    {

        LvlProgressImg.fillAmount +=1/_firedestroyed ;
    }
}
