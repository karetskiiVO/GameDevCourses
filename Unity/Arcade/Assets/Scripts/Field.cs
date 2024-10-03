using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Life {

public class Field : MonoBehaviour {
    [SerializeField]GameObject creator = null;
    Tile[] map;

    [SerializeField]bool create = false;
    bool created = false;

    private void OnValidate() {
        if (created != create) {
            created = create;

            if (created) {
                map = creator.GetComponent<IFieldCreator>().CreateField(this);
            } else {
                Clear();
            }
        }
    }

    private void Start() {
        var meshRenderer = GetComponent<MeshRenderer>();

        foreach (var tile in map) {
            
        }
    }

    private void Clear() {
        if (map != null) Array.Clear(map, 0, map.Length);
        GetComponent<MeshFilter>().sharedMesh.Clear();
    }
}

}