using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

public class Tile {
    private IFraction next = null;
    private readonly int[] polygonidx;
    public IFraction fraction = null;
    public Tile[] neightbours;

    public void Interact () {
        next = fraction.Interact(this);
    }

    public void Flush () {
        if (!ReferenceEquals(fraction, next)) {
            next = fraction;
            foreach (var idx in polygonidx) fraction.AddUpdates(idx);
        }
    }

    public void Init (IFraction fraction) {
        this.fraction = fraction;
        foreach (var idx in polygonidx) fraction.AddUpdates(idx);
    }

    public Tile (int[] polygonidx) {
        this.polygonidx = (int[])polygonidx.Clone();
    }
    public Tile (int[] polygonidx, Tile[] neightbours) {
        this.polygonidx = (int[])polygonidx.Clone();
        this.neightbours = (Tile[])neightbours.Clone();
    }
}
