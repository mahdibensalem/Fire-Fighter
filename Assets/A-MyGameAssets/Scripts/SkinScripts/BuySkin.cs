using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuySkin : MonoBehaviour
{
	public Button yourButton;
    private void Awake()
    {
        yourButton = GetComponent<Button>();
    }
    void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(GetComponentInParent<SkinInShop>().OnButtonPress);
	}
}
