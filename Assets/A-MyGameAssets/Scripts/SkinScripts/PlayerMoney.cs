using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance;

    [SerializeField] private int playerMoney;//SFD
    public TextMeshProUGUI[] MoneyTXT=new TextMeshProUGUI[2];

    private const string prefMoney = "Coin";

    private void Awake()
    {
        Instance = this;

        playerMoney = PlayerPrefs.GetInt(prefMoney,playerMoney);
        UpdateCoinsText();
    }

    public bool TryRemoveMoney(int moneyToRemove)
    {
        if (playerMoney >= moneyToRemove)
        {
            playerMoney -= moneyToRemove;
            PlayerPrefs.SetInt(prefMoney, playerMoney);
            UpdateCoinsText();
            return true;
        }
        else
        {
            return false;
        }
    }
    void UpdateCoinsText()
    {
        foreach(TextMeshProUGUI txt in MoneyTXT)
        txt.text = playerMoney.ToString();
    }
    public void BuyHealth(int value)
    {
        if (TryRemoveMoney(value))
        {
            //EnergyManager.instance.UpdateFullHealth();
        }
    }


}