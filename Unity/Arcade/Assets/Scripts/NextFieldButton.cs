using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextFieldButton : MonoBehaviour {
    [SerializeField]GameObject[] creators;
    int currentIdx = 0;

    Field field = null;

    private void Start () {
        field = GameObject.Find("Field").GetComponent<Field>();
        field.NewGame(creators[currentIdx]);
    }

    public void Change () {
        currentIdx = (currentIdx + 1) % creators.Length;
        field.NewGame(creators[currentIdx]);
    }
}
