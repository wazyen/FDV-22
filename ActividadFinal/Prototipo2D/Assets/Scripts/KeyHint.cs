using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class KeyHint : MonoBehaviour
{
    public CinemachineVirtualCamera _keyCamera;
    public Key _key;

    private bool _isKeyPicked = false;
    
    void Start()
    {
        _key.OnPicked += OnKeyPicked;
    }

    void OnKeyPicked(Pickable pickable)
    {
        _isKeyPicked = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isKeyPicked && collision.gameObject.tag == "Player")
        {
            _keyCamera.enabled = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isKeyPicked && collision.gameObject.tag == "Player")
        {
            _keyCamera.enabled = false;
        }
    }
}
