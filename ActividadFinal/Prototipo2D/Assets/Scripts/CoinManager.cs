using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public Object CoinPrefab;
    
    public float CoinDropRate = 1.0f;
    public float CoinDropHorizontalRange = 7.0f;
    public float CoinTTL = 7.0f;
    
    public Stack<Coin> _coinPool;
    
    public int MaxCoinsInstantiated = 7;
    private int _curCoinsInstantiated = 0;
    
    public delegate void NewCoinInstantiated(Coin coin);
    public event NewCoinInstantiated OnNewCoinInstantiated;
    
    void Start()
    {
        _coinPool = new Stack<Coin>();
        InvokeRepeating("DropCoin", CoinDropRate, CoinDropRate);
    }

    void DropCoin()
    {
        Debug.Log("Coin Pool item count: " + _coinPool.Count);
        if (_coinPool.Count == 0 && CoinPrefab != null)
        {
            if (_curCoinsInstantiated < MaxCoinsInstantiated)
            {
                Debug.Log("Coin Pool empty. Instantiating a new coin...");
                Object instCoinObject = Instantiate(CoinPrefab, GetRandomLocationForCoin(), new Quaternion());
                Coin instCoin = instCoinObject.GetComponent<Coin>();
                if (instCoin != null)
                {
                    instCoin.TimeToLive = CoinTTL;
                    instCoin.OnExpired += HideCoin;
                    if (OnNewCoinInstantiated != null)
                    {
                        OnNewCoinInstantiated(instCoin);
                    }
                }
                ++_curCoinsInstantiated;
            }
        }
        else
        {
            Debug.Log("Coin Pool not empty. Reusing a coin from the Coin Pool...");
            Coin popCoin = _coinPool.Pop();
            popCoin.TimeToLive = CoinTTL;
            popCoin.transform.position = GetRandomLocationForCoin();
            popCoin.gameObject.SetActive(true);
        }
        
    }

    void HideCoin(Pickable pickable)
    {
        Debug.Log("Coin expired. Hiding it...");
        Coin coin = pickable as Coin;
        if (coin != null)
        {
            coin.gameObject.SetActive(false);
            _coinPool.Push(coin);
        }
    }

    Vector3 GetRandomLocationForCoin()
    {
        return new Vector3
        (
            transform.position.x + Random.Range(-CoinDropHorizontalRange, CoinDropHorizontalRange),
            transform.position.y,
            transform.position.z
        );
    }
}
