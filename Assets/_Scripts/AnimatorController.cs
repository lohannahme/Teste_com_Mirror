using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class AnimatorController : NetworkBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private NetworkAnimator _networkAnimator = null;

    private Vector2 _moveValue;

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
    }

    [ClientCallback]
    private void OnEnable()
    {
        Controls.Enable();
        Controls.Player.Move.performed += ctx => ReadMoveValue(ctx);
        Controls.Player.Move.canceled += ctx => ReadMoveValue(ctx);

        Controls.Player.Jump.performed += ctx => JumpAnimation();
    }

    [ClientCallback]
    private void OnDisable()
    {
        Controls.Disable();
        Controls.Player.Move.performed -= ctx => ReadMoveValue(ctx);
        Controls.Player.Move.canceled -= ctx => ReadMoveValue(ctx);

        Controls.Player.Jump.performed -= ctx => JumpAnimation();

    }

    [ClientCallback]
    private void Update() => Animations();

    [ClientCallback]
    private void JumpAnimation()
    {
        _networkAnimator.SetTrigger("Jump");
    }


    [Client]
    private void Animations()
    {
        if (_moveValue.x > 0 || _moveValue.y > 0 || _moveValue.x < 0 || _moveValue.y < 0)
        {
            _animator.SetBool("Walk", true);
        }
        else if (_moveValue.x == 0 || _moveValue.y == 0)
        {
            _animator.SetBool("Walk", false);
        }
    }


    private void ReadMoveValue(InputAction.CallbackContext ctx)
    {
        _moveValue = ctx.ReadValue<Vector2>();
    }

    
}
