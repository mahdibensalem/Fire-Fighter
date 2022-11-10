using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    public static GameObject EquipedSkin { get; private set; }
    public static GameObject[] EquipedAbilities;

    [SerializeField] private BoxDB[] allSkins;

    [SerializeField] private Transform skinsInShopPanelsParent;
    [SerializeField] private List<SkinInShop> skinsInshopPanels = new List<SkinInShop>();

    [SerializeField] private Button currentlyEquippedSkinButton;
    private void Awake()
    {
        Instance = this;
        foreach (Transform s in skinsInShopPanelsParent)
        {
            if (s.TryGetComponent(out SkinInShop skinInShop))
                skinsInshopPanels.Add(skinInShop);
        }
        EquipPreviousSkin();

        SkinInShop skinEquipedPanel = Array.Find(skinsInshopPanels.ToArray(), dunnyFind => dunnyFind._BoxInfo._skinModel == EquipedSkin);
        currentlyEquippedSkinButton = skinEquipedPanel.GetComponentInChildren<Button>();
        currentlyEquippedSkinButton.interactable = true;//kénet false
    }
    private void EquipPreviousSkin()
    {
        int lastSkinUsed = PlayerPrefs.GetInt("skinPref", 1);
        SkinInShop skinEquipedPanel = Array.Find(skinsInshopPanels.ToArray(), dunnyFind => dunnyFind._BoxInfo._skinId == lastSkinUsed);
        EquipSkin(skinEquipedPanel);
    }

    public void EquipSkin(SkinInShop skinInfoInshop)
    {
        EquipedSkin = skinInfoInshop._BoxInfo._skinModel;
        //EquipedAbilities = skinInfoInshop._BoxInfo.abilities;
        PlayerPrefs.SetInt("skinPref", skinInfoInshop._BoxInfo._skinId);
        if (currentlyEquippedSkinButton != null)
        {
            currentlyEquippedSkinButton.interactable = true;
            currentlyEquippedSkinButton.GetComponent<Image>().sprite = currentlyEquippedSkinButton.GetComponentInParent<SkinInShop>().equipe;
        }

        currentlyEquippedSkinButton = skinInfoInshop.GetComponentInChildren<Button>();
        currentlyEquippedSkinButton.interactable = true;//kénet false
        currentlyEquippedSkinButton.GetComponent<Image>().sprite = skinInfoInshop.equiped;
    }
}
