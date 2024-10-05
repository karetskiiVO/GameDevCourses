using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Field : MonoBehaviour {
    [Serializable]
    struct FractionConfig {
        public GameObject fractionObj;
        public Material material;
    }
    struct FractionInfo {
        public IFraction fraction;
        public int subMeshIdx;
    }

    [SerializeField]FractionConfig[] fractionObj;
    [SerializeField]GameObject creator = null;

    private FractionInfo[] fractionsInfo;
    private MeshRenderer meshRenderer = null;
    private Mesh mesh = null; 
    private Tile[] map;

    // [SerializeField]bool create = false;
    // bool created = false;
    //
    // private void OnValidate() {
    //     if (created != create) {
    //         created = create;
    //
    //         if (created) {
    //             Spawn();
    //         } else {
    //             Clear();
    //         }
    //     }
    // }

    public void Spawn () {
        map = creator.GetComponent<IFieldCreator>().CreateField(this);
    }

    private void Start() {
        Spawn();
        
        meshRenderer ??= GetComponent<MeshRenderer>();
        mesh ??= GetComponent<MeshFilter>().mesh;

        fractionsInfo = new FractionInfo[fractionObj.Length];
        var usedMaterials = new Material[fractionObj.Length];
        for (var i = 0; i < fractionObj.Length; i++) {
            fractionsInfo[i].fraction = fractionObj[i].fractionObj.GetComponent<IFraction>();
            fractionsInfo[i].subMeshIdx = i;

            usedMaterials[i] = fractionObj[i].material;
        }
        
        meshRenderer.materials = usedMaterials;
        mesh.subMeshCount = usedMaterials.Length;

        foreach (var tile in map) {
            tile.Init(fractionsInfo[0].fraction); 
        }

        foreach (var fractionInfo in fractionsInfo) {
            mesh.SetTriangles(fractionInfo.fraction.FlushUpdates(), fractionInfo.subMeshIdx);
        }
        // new thread
    }
    
    private void Loop () {
        foreach (var tile in map) tile.Interact();
        foreach (var tile in map) tile.Flush();

        foreach (var fractionInfo in fractionsInfo) {
            mesh.SetTriangles(fractionInfo.fraction.FlushUpdates(), fractionInfo.subMeshIdx);
        }
    }

    private void Clear() {
        if (map != null) Array.Clear(map, 0, map.Length);
        GetComponent<MeshFilter>().sharedMesh.Clear();
    }
}
