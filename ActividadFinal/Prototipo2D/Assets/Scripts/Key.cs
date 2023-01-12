using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Pickable
{
    public GameObject Door;

    protected override void BePicked()
    {
        Destroy(Door);
        base.BePicked();
    }
}
