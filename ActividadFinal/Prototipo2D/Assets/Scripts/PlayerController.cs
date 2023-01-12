using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 3.0f;
    public float JumpForce = 500.0f;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Rigidbody2D _rigidBody2D;
    
    private bool _canJump = true;
    
    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
        _rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Manage animations
        if (_animator)
        {
            _animator.SetBool("IsWalking", horizontalInput != 0);
        }

        // Manage sprite orientation
        if (_spriteRenderer)
        {
            _spriteRenderer.flipX = horizontalInput == 0.0f ? _spriteRenderer.flipX : horizontalInput < 0.0f;
        }

        // Manage movement
        Vector3 movement = horizontalInput * MovementSpeed * Vector3.right * Time.deltaTime;
        transform.Translate(movement);

        // Manage jump
        if (_rigidBody2D && Mathf.Abs(_rigidBody2D.velocity.y) < 0.01f && _canJump && Input.GetAxis("Jump") > 0.0f)
        {
            _canJump = false;
            _rigidBody2D.AddForce(JumpForce * Vector2.up);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _canJump = true;
        }
    }
}