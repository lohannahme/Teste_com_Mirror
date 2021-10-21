using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PepinoPass : NetworkBehaviour
{
    [SerializeField] private GameObject _pepino = null;
    [SerializeField] private bool _hasPepino = false;

    private bool _isPressed;

    private GameObject _playerAdversary = null;

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
        _pepino.gameObject.SetActive(false);
    }

    [ClientCallback]
    private void OnEnable()
    {
        Controls.Enable();

        Controls.Player.PassPepino.performed += ctx => PassPepino();
        Controls.Player.PassPepino.canceled += ctx => PassPepino();

    }

    [ClientCallback]
    private void OnDisable()
    {
        Controls.Disable();

        Controls.Player.PassPepino.performed -= ctx => PassPepino();
        Controls.Player.PassPepino.canceled -= ctx => PassPepino();
    }

    [Command]
    private void PassPepino()
    {
        if(_playerAdversary != null)
        {
            _playerAdversary.GetComponent<PepinoPass>().ReceivePepino();
            Debug.Log("passou");
            DontHavePepino();
        }
    }

    [ClientRpc]
    private void DontHavePepino()
    {
        _hasPepino = false;
        _pepino.gameObject.SetActive(false);
    }

    [ClientCallback]
    private void OnTriggerEnter(Collider other)
    {
        _playerAdversary = other.gameObject;
        Debug.Log("player adv is :" + _playerAdversary);
    }

    [ClientCallback]
    private void OnTriggerExit(Collider other)
    {
        _playerAdversary = null;
        Debug.Log("player adv is :" + _playerAdversary);
    }


    [ClientRpc]
    public void ReceivePepino()
    {
        _hasPepino = true;
        _pepino.gameObject.SetActive(true);

    }

}
