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
    [SerializeField] private Image buyZone;
    public Sprite equipe,equiped;

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
            //coinImage.SetActive(false);
            //buyZone.sprite = equiped;
            //buttonText.text = "Equip";
        }
        else
        {
            buyZone.sprite = BoxInfo._PriceImage;

            //buttonText.text = BoxInfo._skinPrice.ToString();
            //coinImage.SetActive(true);
        }
    }

    // Update is called once per frame
    public void OnButtonPress()
    {
        if (isSkinUnlocked)
        {
            //equip
            SkinManager.Instance.EquipSkin(this);
            //buyZone.sprite = equiped;
        }
        else
        {
            //buy
            if (PlayerMoney.Instance.TryRemoveMoney(BoxInfo._skinPrice))
            {
                isSkinUnlocked = true;
                PlayerPrefs.SetInt(BoxInfo._skinId.ToString(), 1);

                buyZone.sprite = equipe;

                //buttonText.text = "Equip";
                //coinImage.SetActive(false);
            }
            else
            {
                buyZone.sprite = BoxInfo._PriceImage;
                //buttonText.text =BoxInfo._skinPrice.ToString();
                //coinImage.SetActive(true);
            };
        }
    }
    public void Equipe()
    {
        buyZone.sprite = equipe;
    }
}
