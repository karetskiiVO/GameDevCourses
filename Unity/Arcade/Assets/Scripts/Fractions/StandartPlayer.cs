using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartPlayer : Fraction {
    public override Fraction Interact (Tile tile) {
        int cnt = 0;
        Fraction res = this;
        foreach(var neight in tile.neightbours) {
            if (ReferenceEquals(neight.fraction, this)) cnt++;
        }

        if (cnt < 2 || cnt > 3) {
            res = field.Filler.DefaultFraction;
        }

        return res;
    }
}
