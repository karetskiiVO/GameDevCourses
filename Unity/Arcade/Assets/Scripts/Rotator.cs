using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Rotator : MonoBehaviour {
    [SerializeField]float rotationSpeed = 0.2f;
    [SerializeField]Vector3 axis = Vector3.up;

    UnityEngine.Transform tr = null;

    void Start () {
        tr ??= this.transform;
    }

    void Update () {
        tr.Rotate(axis, rotationSpeed * Time.deltaTime);
    }
}
