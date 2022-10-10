using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    private static int _Coins ;
    private static int _diamonds ;
    // Start is called before the first frame update
    
    static GameData()
    {
        _Coins = PlayerPrefs.GetInt("Coins", 0);
        _diamonds = PlayerPrefs.GetInt("Diamonds", 0);
    }
    public static int Coins
    {
        get { return _Coins; }
        set { PlayerPrefs.SetInt("Coins", (_Coins = value)); }
    }
    public static int Diamonds
    {
        get { return _diamonds; }
        set { PlayerPrefs.SetInt("Diamonds", (_diamonds= value)); }
    }

}
