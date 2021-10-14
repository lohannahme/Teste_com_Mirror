using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private CharacterController _controller = null;
    
    private bool _groundedPlayer;

    private float _jumpHeight = 3f;
    private float _gravityValue = -9.81f;

    private Vector3 _playerVelocity;

    private Vector2 _previousInput;

    private Controls _controls;
    private Controls Controls
    {
        get
        {
            if (_controls != null) { return _controls; }
            return _controls = new Controls();
        }

    }

    public override void OnStartAuthority()
    {
        enabled = true;

        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Controls.Player.Move.canceled += ctx => ResetMovement();

        Controls.Player.Jump.performed += ctx => DoJump();
        Controls.Player.Jump.canceled -= ctx => DoJump();
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();

    [ClientCallback]
    private void OnDisable() => Controls.Disable();

    [ClientCallback]
    private void Update()
    {
        Move();
        
    }

    [Client]
    private void SetMovement(Vector2 movement) => _previousInput = movement;

    [Client]
    private void ResetMovement() => _previousInput = Vector2.zero;

    [Client]
    private void Move()
    {
        Vector3 right = _controller.transform.right;
        Vector3 forward = _controller.transform.forward;

        right.y = 0;
        forward.y = 0;

        Vector3 movement = right.normalized * _previousInput.x + forward.normalized * _previousInput.y;

        _controller.Move(movement * _movementSpeed * Time.deltaTime);

        _groundedPlayer = _controller.isGrounded;

        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0;
        }

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    [ClientCallback]
    private void DoJump()
    {
        _groundedPlayer = _controller.isGrounded;
        Debug.Log(_groundedPlayer);

        if (_groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
            Debug.Log("funcionabitch");
        }
        
    }

}
