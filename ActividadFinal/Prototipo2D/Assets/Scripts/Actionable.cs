using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actionable : MonoBehaviour
{
    public int RequiredCoins = 0;
    private int _collectedCoins = 0;
    
    private Chaser _chaser;
    public GameObject Hint;
    
    void Start()
    {
        _chaser = gameObject.GetComponent<Chaser>();
        
        Pickable[] pickables = FindObjectsOfType<Pickable>();
        foreach (Pickable pickable in pickables)
        {
            SubscribeOnPicked(pickable);
        }
        CoinManager[] coinManagers = FindObjectsOfType<CoinManager>();
        foreach (CoinManager coinManager in coinManagers)
        {
            coinManager.OnNewCoinInstantiated += SubscribeOnPicked;
        }
    }

    void Collect(Pickable pickable)
    {
        Coin coin = pickable as Coin;
        if (coin != null)
        {
            if (++_collectedCoins >= RequiredCoins)
            {
                if (Hint)
                {
                    Destroy(Hint);
                }

                _chaser.SetEnabled(true);
                Destroy(this);
            }
        }
    }

    void SubscribeOnPicked(Pickable pickable)
    {
        pickable.OnPicked += Collect;
    }
}
