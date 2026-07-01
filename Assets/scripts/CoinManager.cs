using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
   
    public static int coinCount;
    private Text coinText;

    private void Awake()
    {
        instance = this;
        coinCount = 0;
        DontDestroyOnLoad(gameObject);

        coinText = GetComponentInChildren<Text>();
        coinText.text = coinCount.ToString();
    }
    public static void setCoinCt(int num)
    {
        coinCount = num;
        if (instance != null)
            instance.coinText.text = coinCount.ToString();
    }

    public static int getCoinCt()
    {
        return coinCount;
    }

    public void AddCoin()
    {
        coinCount ++;
        coinText.text = coinCount.ToString();
    }
}
