using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
   
    public static int coinCount;

    private void Awake()
    {
        instance = this;
        coinCount = 0;
        DontDestroyOnLoad(gameObject);
    }
    public static void setCoinCt(int num)
    {
        coinCount = num;
    }

    public static int getCoinCt()
    {
        return coinCount;
    }

    public void AddCoin()
    {
        coinCount = coinCount + 1;
      
    }
}
