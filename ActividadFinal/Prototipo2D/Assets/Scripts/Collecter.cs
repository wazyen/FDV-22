using TMPro;
using UnityEngine;

public class Collecter : MonoBehaviour
{
    public TMP_Text NKeysUILabel;
    public TMP_Text NCoinsUILabel;
    
    private int KeysCollected = 0;
    private int CoinsCollected = 0;

    void Start()
    {
        Pickable[] pickables = GameObject.FindObjectsOfType<Pickable>();
        foreach (Pickable pickable in pickables)
        {
            SubscribeOnPicked(pickable);
        }
        CoinManager[] coinManagers = GameObject.FindObjectsOfType<CoinManager>();
        foreach (CoinManager coinManager in coinManagers)
        {
            coinManager.OnNewCoinInstantiated += SubscribeOnPicked;
        }
    }

    void Collect(Pickable pickable)
    {
        Key key = pickable as Key;
        if (key != null)
        {
            KeysCollected++;
            NKeysUILabel.text = "x" + KeysCollected;
            return;
        }
        Coin coin = pickable as Coin;
        if (coin != null)
        {
            CoinsCollected++;
            NCoinsUILabel.text = "x" + CoinsCollected;
            return;
        }
    }

    void SubscribeOnPicked(Pickable pickable)
    {
        pickable.OnPicked += Collect;
    }
}
