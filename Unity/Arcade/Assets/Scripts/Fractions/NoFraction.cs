using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using MiscUtil.Collections.Extensions;
using System.Runtime.InteropServices.ComTypes;

// TODO рандомный прирост
public class NoFraction : MonoBehaviour, IFraction {
    public DynamicBuffer<int> updates = new DynamicBuffer<int>();

    private readonly Dictionary<IFraction, int> buffer;

    public IFraction Interact (Tile tile) {
        int cnt = 0;
        IFraction res = this;
        foreach(var neight in tile.neightbours) {
            if (ReferenceEquals(neight.fraction, this)) cnt++;

            buffer[neight.fraction]++;
        }

        if (cnt == 3) {
            foreach (var key in buffer.Keys) {
                if (buffer[key] >= 2) {
                    res = key;
                    break;
                }
            }
        }

        foreach (var key in buffer.Keys) {
            buffer[key] = 0;
        }
        return res;
    }

    public int[] FlushUpdates () {
        var res = new int[updates.Count];
        var buf = updates.ToArray();
        for (var i = 0; i < updates.Count; i++) {
            res[i] = buf[i];
        }
        updates.Clear();
        return res;
    }

    public void AddUpdates(int upd) {
        updates.Add(upd);
    }
}
