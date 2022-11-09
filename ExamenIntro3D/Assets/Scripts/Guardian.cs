using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Guardian : MonoBehaviour
{
    private Chaser _chaser;
    
    void Start()
    {
        _chaser = gameObject.GetComponent<Chaser>();
    }

    public void StartProtect(Transform treasure)
    {
        if (!_chaser)
        {
            return;
        }
        _chaser.Goals = new Transform[1];
        _chaser.Goals[0] = treasure;
    }
    
    public void StopProtect(Transform treasure)
    {
        if (!_chaser)
        {
            return;
        }
        _chaser.Goals = new Transform[0];
    }
}
