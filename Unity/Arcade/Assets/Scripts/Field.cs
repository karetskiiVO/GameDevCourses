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
    private DateTime lastUpdate = DateTime.Now;

    public Tile[] map;
    public Tile[] tilesFromEdgeIdeces;

    private FieldFiller.FractionInfo[] fractionsInfo;

    public FieldFiller Filler{get => filler;}

    [SerializeField]bool create = false;
    [SerializeField]bool paused = false;

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

    private void Spawn () {
        creator.GetComponent<FieldCreator>().CreateField(this);
    }

    private void Start () {
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
        
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;

        var usedMaterials = filler.Init(this);
        fractionsInfo = filler.fractionsInfo;

        meshRenderer.materials = usedMaterials;
        mesh.subMeshCount = usedMaterials.Length;

        Filler.DefaultFill();

        Flush();

        initiated = true;
    }

    public void Fill () {
        Filler.Fill();
        Flush();
    }
    private void DefaultFill () { Filler.DefaultFill(); }
    private void FixedUpdate () {
        var now = DateTime.Now;

        if (!initiated) return;
        if (!paused && (now - lastUpdate < stepTime)) return;

        Step();
        lastUpdate = now;
    }
    
    private void Step () {
        foreach (var tile in map) tile.Interact();
        foreach (var tile in map) tile.Flush(!paused);

        Flush();
    }
    private void Flush () {
        foreach (var fractionInfo in fractionsInfo) {
            mesh.SetTriangles(fractionInfo.fraction.FlushUpdates(), fractionInfo.subMeshIdx);
        }
    }
    private void Clear () {
        if (map != null) Array.Clear(map, 0, map.Length);
        GetComponent<MeshFilter>().sharedMesh.Clear();
    }

    public Tile GetTileFromEdgeIdx (int idx) {
        return tilesFromEdgeIdeces[idx];
    }

    public void PauseGame () { paused = true; }
    public void StartGame () { paused = false; }

    public void SwitchPause () { paused = !paused; }

    public bool Paused{ get => paused; }

    public void NewGame () { NewGame(null); }
    public void NewGame (GameObject creator) {
        if (creator == null || ReferenceEquals(creator, this.creator)) {
            PauseGame();
            DefaultFill();
            StartGame();
        } else {
            this.creator = creator;

            PauseGame();
            Clear();
            Spawn();
            DefaultFill();
            StartGame();
        }
    }
}
