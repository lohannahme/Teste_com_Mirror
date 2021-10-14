using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Mirror;
using UnityEngine.InputSystem;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private Transform _playerTransform = null;

    private float _clampAngle = 80f;
    private float _speed = 7f;

    private Vector3 _startingRotation;
    private Vector2 _deltaInput;

    private Controls _controls;

    private Controls Controls
    {
        get
        {
            if (_controls != null) { return _controls; }
            return _controls = new Controls();
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    [ClientCallback]
    protected override void OnEnable()
    {
        Controls.Enable();
        Controls.Player.Look.performed += ctx => HandleLookInput(ctx);
        Controls.Player.Look.canceled += ctx => HandleLookInput(ctx);
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (_startingRotation == null) _startingRotation = transform.localRotation.eulerAngles;

                _startingRotation.x += _deltaInput.x * _speed * Time.deltaTime;
                _startingRotation.y += _deltaInput.y * _speed * Time.deltaTime;

                _playerTransform.Rotate(0f, _deltaInput.x * _speed * Time.deltaTime, 0f);

                _startingRotation.y = Mathf.Clamp(_startingRotation.y, -_clampAngle, _clampAngle);
                state.RawOrientation = Quaternion.Euler(-_startingRotation.y, _startingRotation.x, 0f);
            }
        }
    }

    [ClientCallback]
    private void OnDisable()
    {
        Controls.Enable();
        Controls.Player.Look.performed -= ctx => HandleLookInput(ctx);
        Controls.Player.Look.canceled -= ctx => HandleLookInput(ctx);
    }

    private void HandleLookInput(InputAction.CallbackContext ctx)
    {
        _deltaInput = ctx.ReadValue<Vector2>();
    }
}
