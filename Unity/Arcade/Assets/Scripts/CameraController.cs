using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Transform tr;

    void Start () {
        tr = gameObject.transform;
    }

    // Update is called once per frame
    void Update () {
        var movementVector = Vector3.right * Input.GetAxisRaw("Horizontal") + Vector3.forward * Input.GetAxisRaw("Vertical");
        movementVector = movementVector.normalized * 0.1f;

        tr.position += tr.right * movementVector.x + tr.forward * movementVector.z + tr.up * movementVector.y;
    }
}
