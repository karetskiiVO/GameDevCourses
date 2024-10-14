using UnityEngine;

public class StandartPlayer : Fraction {
    private int newInstanceCount = 0;
    [SerializeField]ProgressWriter writer = null;

    public override int[] FlushUpdates () {
        if (writer != null) {
            writer.AddScore(newInstanceCount);
            newInstanceCount = 0;
        }

        return base.FlushUpdates();
    }

    public override void AddTileToUpdates (Tile tile, Fraction prev) {
        if (!ReferenceEquals(this, prev)) newInstanceCount++;

        base.AddTileToUpdates(tile, prev);
    }

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