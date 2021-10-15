using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RotatePepino : NetworkBehaviour
{ 
    [ServerCallback]
    void Update()
    {
        transform.Rotate(new Vector3(0f, 100f, 0f) * Time.deltaTime);
    }

    [ClientCallback]
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }
}
