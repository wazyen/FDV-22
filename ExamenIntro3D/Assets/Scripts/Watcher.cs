using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    public Transform Treasure;
    public Transform Threat;
    public float SafeDistance = 5.0f;

    private bool _wasSafe;
    
    void Start()
    {
        _wasSafe = CheckSafe();
    }
    
    void Update()
    {
        bool itsSafe = CheckSafe();
        if (_wasSafe && !itsSafe)
        {
            foreach (Guardian guardian in GameObject.FindObjectsOfType<Guardian>())
            {
                guardian.StartProtect(Treasure);
            }
            foreach (Patrol patrol in GameObject.FindObjectsOfType<Patrol>())
            {
                patrol.StartProtect();
            }
        }
        else if (!_wasSafe && itsSafe)
        {
            foreach (Guardian guardian in GameObject.FindObjectsOfType<Guardian>())
            {
                guardian.StopProtect(Treasure);
            }
            foreach (Patrol patrol in GameObject.FindObjectsOfType<Patrol>())
            {
                patrol.StopProtect();
            }
        }
        _wasSafe = itsSafe;
    }

    bool CheckSafe()
    {
        return Vector3.Distance(Treasure.position, Threat.position) >= SafeDistance;
    }
}
