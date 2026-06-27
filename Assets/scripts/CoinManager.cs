using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public Text coinText;

    private int coinCount;

    private void Awake()
    {
        instance = this;
        coinCount = 0;
    }

    public void AddCoin()
    {
        coinCount = coinCount + 1;
        coinText.text = "Coins: " + coinCount.ToString();
    }
}
