using System.Collections.Generic;

public class Tile {
    private Fraction next = null;
    public readonly int[] polygonidx;
    public Fraction fraction = null;
    public Tile[] neightbours;

    public void Interact () {
        next = fraction.Interact(this);
    }

    public void Flush (bool tonext = true) {
        var prev = fraction;
        if (tonext) fraction = next;
        fraction.AddTileToUpdates(this, prev);
    }

    public void Init (Fraction fraction) {
        next = fraction;
        Flush();
    }

    public Tile (List<int> polygonidx) {
        this.polygonidx = new int[polygonidx.Count];
        polygonidx.CopyTo(this.polygonidx, 0);
    }

    public Tile (List<int> polygonidx, Tile[] neightbours) {
        this.polygonidx = new int[polygonidx.Count];
        polygonidx.CopyTo(this.polygonidx, 0);
        this.neightbours = (Tile[])neightbours.Clone();
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
