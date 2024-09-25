using System.Collections.Generic;
using UnityEditor;

namespace Life {

public abstract class Fraction {
    public abstract Fraction Process (Tile tile);

    public static Fraction noFraction{get;} = new NoFractionType();
}

public class NoFractionType : Fraction {
    public override Fraction Process (Tile tile) {
        return this;
    }
}

public class Tile {
    public Fraction fraction = Fraction.noFraction;
    private Fraction nextFraction = Fraction.noFraction;
    public Tile[] neightbours;

    public void Process () {}

    public void Update () {
        fraction = nextFraction;
        nextFraction = Fraction.noFraction;
        
        Draw();
    }

    private void Draw () {} 

    public Tile () {}
    public Tile (Tile[] neightbours) {
        this.neightbours = (Tile[])neightbours.Clone();
    }
}

}