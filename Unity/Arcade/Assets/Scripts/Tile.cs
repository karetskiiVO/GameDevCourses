using System.Collections.Generic;
using UnityEngine;
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
            fraction = next;
            foreach (var idx in polygonidx) fraction.AddUpdates(idx);
        }
    }

    public void Init (IFraction fraction) {
        next = fraction;
        Flush();
    }

    public Tile (int[] polygonidx) {
        this.polygonidx = new int[polygonidx.Length];
        polygonidx.CopyTo(this.polygonidx, 0);
    }

    public Tile (int[] polygonidx, Tile[] neightbours) {
        this.polygonidx = new int[polygonidx.Length];
        polygonidx.CopyTo(this.polygonidx, 0);
        this.neightbours = (Tile[])neightbours.Clone();
    }
}
