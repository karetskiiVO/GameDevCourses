using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Field : MonoBehaviour {
    [SerializeField]GameObject creator = null;
    [SerializeField]GameObject fillerObj = null;
    // time controller
    [SerializeField]int updateSeconds = 1;
    [SerializeField]int updateMilliSeconds = 0;

    private TimeSpan stepTime;
    private MeshRenderer meshRenderer = null;
    private Mesh mesh = null; 
    private FieldFiller filler = null;

    private Tile[] map;
    private FieldFiller.FractionInfo[] fractionsInfo;

    public FieldFiller Filler{get => filler;}

    [SerializeField]bool create = false;
    bool created = false;

    private bool initiated = false;

    private void OnValidate() {
        if (created != create) {
            created = create;

            if (created) {
                Spawn();
            } else {
                Clear();
            }
        }
    }

    public void Spawn () {
        map = creator.GetComponent<FieldCreator>().CreateField(this);
    }

    private void Start() {
        Spawn();
        
        stepTime = new TimeSpan(
            days:         0,
            hours:        0,
            minutes:      0,
            seconds:      updateSeconds, 
            milliseconds: updateMilliSeconds
        );

        meshRenderer = GetComponent<MeshRenderer>();
        mesh = GetComponent<MeshFilter>().mesh;
        filler = fillerObj.GetComponent<FieldFiller>();

        var usedMaterials = filler.Init(this);
        fractionsInfo = filler.fractionsInfo;

        meshRenderer.materials = usedMaterials;
        mesh.subMeshCount = usedMaterials.Length;

        foreach (var tile in map) {
            tile.Init(Filler.Generate(tile)); 
        }

        foreach (var fractionInfo in fractionsInfo) {
            mesh.SetTriangles(fractionInfo.fraction.FlushUpdates(), fractionInfo.subMeshIdx);
        }

        initiated = true;
    }

    private DateTime lastUpdate = DateTime.Now;
    private void FixedUpdate () {
        var now = DateTime.Now;

        if (!initiated) return;
        if (now - lastUpdate < stepTime) return;
        Step();
        lastUpdate = now;
    }
    
    private void Step () {
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
