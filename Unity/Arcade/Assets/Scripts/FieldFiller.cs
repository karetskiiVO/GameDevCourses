using System;
using UnityEngine;

public abstract class FieldFiller : MonoBehaviour {
    [Serializable]
    protected struct FractionConfig {
        public GameObject fractionObj;
        public Material material;
    }
    public struct FractionInfo {
        public Fraction fraction;
        public int subMeshIdx;
    }

    [SerializeField]protected int defaultFractionIdx = 0;
    [SerializeField]FractionConfig[] fractionObj;

    public int DefaultFractionIdx{ get => defaultFractionIdx; }

    public FractionInfo[] fractionsInfo;
    protected Field field;

    public Fraction DefaultFraction{ get => fractionsInfo[defaultFractionIdx].fraction; }

    public virtual Material[] Init (Field field) {
        this.field = field;

        fractionsInfo = new FractionInfo[fractionObj.Length];
        var usedMaterials = new Material[fractionObj.Length];

        for (var i = 0; i < fractionObj.Length; i++) {
            fractionsInfo[i].fraction = fractionObj[i].fractionObj.GetComponent<Fraction>();
            fractionsInfo[i].subMeshIdx = i;
            
            usedMaterials[i] = fractionObj[i].material;
        }
        
        foreach (var fractionInfo in fractionsInfo) {
            fractionInfo.fraction.Init(field, this);
        }

        return usedMaterials;
    }

    public void DefaultFill () {
        foreach (var tile in field.map) {
            tile.Init(DefaultFraction);
        }
    }

    public abstract void Fill ();
}
