using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject Hint;
    public Chaser[] Platforms;
    public Sprite ActiveSprite;
    public Sprite InactiveSprite;

    private SpriteRenderer _spriteRenderer;
    private bool _isPulled = false;
    private bool _isPlayerReady = false;
    
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (_isPlayerReady && Input.GetKeyDown(KeyCode.E))
        {
            _isPulled = !_isPulled;
            _spriteRenderer.sprite = _isPulled ? ActiveSprite : InactiveSprite;
            for (int i = 0; i < Platforms.Length; ++i)
            {
                Platforms[i].ToggleEnabled();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isPlayerReady = true;
            Hint.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _isPlayerReady = false;
            Hint.SetActive(false);
        }
    }
}
