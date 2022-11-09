using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    private Chaser _chaser;
    
    void Start()
    {
        _chaser = gameObject.GetComponent<Chaser>();
    }

    public void StartProtect()
    {
        if (!_chaser)
        {
            return;
        }
        _chaser.ApplyMovementSpeedModifier(2.0f);
    }
    
    public void StopProtect()
    {
        if (!_chaser)
        {
            return;
        }
        _chaser.ApplyMovementSpeedModifier(0.5f);
    }
}
