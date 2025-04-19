using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager coinManagerInstance { get; private set; }

    int coinCount = 0;

    void Awake()
    {
        if (coinManagerInstance == null)
        {
            coinManagerInstance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;        
    }

    public int GetCoinCount()
    {
        return coinCount;
    }
}
