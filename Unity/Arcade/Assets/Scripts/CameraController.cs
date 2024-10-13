using UnityEngine;

public class CameraController : MonoBehaviour {
    private Transform tr;
    private Field field;

    void Start () {
        tr = gameObject.transform;
        field = GameObject.Find("Field").GetComponent<Field>();
    }

    void Update () {
        field.controller.HandleInput(tr);
    }
}
