using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpawnFireBall : MonoBehaviour
{
    static public SpawnFireBall Instance;
    [SerializeField] private GameObject fireBall,WholeIsland;
    public int fireDestroyed=5;
    [SerializeField] TextMeshProUGUI coinTxt;
    [SerializeField] int amount;
    void Awake()
    {
        Instance = this;
        coinTxt.text = PlayerPrefs.GetInt("Coin").ToString();
    }
    public void SpawnFire()
    {
        GameObject go=Instantiate(fireBall, WholeIsland.transform);
        go.transform.localPosition = new Vector3(transform.position.x, 12f, -40f);
    }
    private void LateUpdate()
    {
        if (fireDestroyed == 0)
        {
            // You  Win
            WholeIsland.GetComponent<PlaneMvmt>().speed = 0;

        }

    }
    public void UpdateCoins()
    {
        int money = PlayerPrefs.GetInt("Coin", (PlayerPrefs.GetInt("Coin"))) + amount;
        PlayerPrefs.SetInt("Coin", money);
        coinTxt.text = money.ToString();
    }
}
