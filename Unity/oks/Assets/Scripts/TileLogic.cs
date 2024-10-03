using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TileLogic : MonoBehaviour {
    public bool masterTalks{get; set;} = false;
    public uint fraction
    {
        get{
            return _fraction;
        }
        set{
            _fraction = value;
            Refresh(masterTalks);
        }
    }
    private uint _fraction = 0;


    GameMasterLogic master = null;

    [SerializeField]Material[] materials;

    void Start () {
        master ??= transform.parent.GetComponent<GameMasterLogic>();
    }

    public void Refresh (bool fromMaster = false) {
        GetComponent<MeshRenderer>().material = materials[fraction];
        if (!fromMaster) master.Refresh();
    }

    void OnMouseUp () {
        if (fraction == 0) {
            fraction = 1;
        }
    }
}
