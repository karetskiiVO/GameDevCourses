using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFiller : FieldFiller {
    readonly System.Random random = new System.Random();

    public override void Fill() {
        foreach (var tile in field.map) {
            tile.Init(fractionsInfo[random.Next(0, fractionsInfo.Length)].fraction);
        }
    }
}
