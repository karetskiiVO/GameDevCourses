using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldFiller : MonoBehaviour {
    [Serializable]
    struct FractionConfig {
        public GameObject fractionObj;
        public Material material;
    }
    public struct FractionInfo {
        public Fraction fraction;
        public int subMeshIdx;
    }

    [SerializeField]int defaultFractionIdx = 0;
    [SerializeField]FractionConfig[] fractionObj;
    public FractionInfo[] fractionsInfo;

    public Fraction DefaultFraction{get => fractionsInfo[defaultFractionIdx].fraction;}
    private readonly System.Random random = new System.Random();

    public Material[] Init (Field field) {
        fractionsInfo = new FractionInfo[fractionObj.Length];
        var usedMaterials = new Material[fractionObj.Length];

        for (var i = 0; i < fractionObj.Length; i++) {
            fractionsInfo[i].fraction = fractionObj[i].fractionObj.GetComponent<Fraction>();
            
            fractionsInfo[i].subMeshIdx = i;
            usedMaterials[i] = fractionObj[i].material;
        }

        foreach (var fractionInfo in fractionsInfo) {
            fractionInfo.fraction.Init(field);
        }

        return usedMaterials;
    }

    public Fraction Generate (Tile _) {
        return fractionsInfo[random.Next(0, fractionsInfo.Length)].fraction;
    }
}
