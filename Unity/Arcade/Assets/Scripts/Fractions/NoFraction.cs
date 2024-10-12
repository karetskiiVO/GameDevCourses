using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MiscUtil.Collections.Extensions;
using System.Runtime.InteropServices.ComTypes;

// TODO рандомный прирост
public class NoFraction : Fraction {
    private Dictionary<Fraction, int> buffer = new Dictionary<Fraction, int>();

    public override void Init (Field field, FieldFiller filler) {
        this.field = field;
        foreach (var fractionInfo in filler.fractionsInfo) buffer[fractionInfo.fraction] = 0;
    }

    public override Fraction Interact (Tile tile) {
        int cnt = 0;
        Fraction res = this;
        foreach(var neight in tile.neightbours) {
            if (neight.fraction == this) continue;
            cnt++;
            buffer[neight.fraction]++;
        }

        if (cnt == 3) {
            foreach (var key in buffer.Keys.ToList()) {
                if (buffer[key] >= 2) {
                    res = key;
                    break;
                }
            }
        }

        foreach (var key in buffer.Keys.ToList()) {
            buffer[key] = 0;
        }

        return res;
    }
}
