using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu]
public class BoxDB : ScriptableObject
{
    [SerializeField] private int skinId;
    public int _skinId { get { return skinId; } }

    [SerializeField] private GameObject skinModel;
    public GameObject _skinModel { get { return skinModel; } }

    [SerializeField] private Sprite skinSprite;
    public Sprite _skinSprite { get { return skinSprite; } }

    [SerializeField] private int skinPrice;
    public int _skinPrice { get { return skinPrice; } }

    //public GameObject[] abilities;

}
