using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public float TimeToLive = 0.0f;
        
    public delegate void Picked(Pickable pickable);
    public event Picked OnPicked;
    public delegate void Expired(Pickable pickable);
    public event Expired OnExpired;

    void Update()
    {
        if (TimeToLive <= 0.0f)
        {
            return;
        }

        TimeToLive -= Time.deltaTime;
        if (TimeToLive <= 0.0f && OnExpired != null)
        {
            OnExpired(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            BePicked();
        }
    }

    protected virtual void BePicked()
    {
        if (OnPicked != null)
        {
            OnPicked(this);
        }
        Destroy(gameObject);
    }
}
