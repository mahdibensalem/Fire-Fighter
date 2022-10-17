using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SkinInShop : MonoBehaviour
{
    [SerializeField]
    private BoxDB BoxInfo;
    public BoxDB _BoxInfo { get { return BoxInfo; } }
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Image skinImage;
     public GameObject coinImage;
    [SerializeField] private bool isSkinUnlocked = false;
    [SerializeField] private bool isFreeSkin;

    void Awake()
    {

        skinImage.sprite = BoxInfo._skinSprite;
        if (isFreeSkin)
        {
            PlayerPrefs.SetInt(BoxInfo._skinId.ToString(),1);
        }
        IsSkinUnlocked();


    }
    private void IsSkinUnlocked()
    {
        if (PlayerPrefs.GetInt(BoxInfo._skinId.ToString()) == 1)
        {
            isSkinUnlocked = true;
            coinImage.SetActive(false);
            buttonText.text = "Equip";
        }
        else
        {
            buttonText.text = BoxInfo._skinPrice.ToString();
            coinImage.SetActive(true);
        }
    }

    // Update is called once per frame
    public void OnButtonPress()
    {
        if (isSkinUnlocked)
        {
            //equip
            SkinManager.Instance.EquipSkin(this);
        }
        else
        {
            //buy
            if (PlayerMoney.Instance.TryRemoveMoney(BoxInfo._skinPrice))
            {
                isSkinUnlocked = true;
                PlayerPrefs.SetInt(BoxInfo._skinId.ToString(), 1);
                buttonText.text = "Equip";
                coinImage.SetActive(false);
            }
            else
            {
                buttonText.text =BoxInfo._skinPrice.ToString();
                coinImage.SetActive(true);
            };
        }
    }
}
