using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class Timer : NetworkBehaviour
{
    [SerializeField] private Text _timeText = null;

    private int _minutes = 2;
    private float _seconds = 5;

    [ClientRpc]
     void Update()
    {
        _seconds -= 1 * Time.deltaTime;

        if (_seconds <= 0)
        {
            _minutes--;
            _seconds = 60;
        }

        _timeText.text = $"{_minutes}:{_seconds.ToString("0")}";

        Debug.Log(_seconds);
    }
}
