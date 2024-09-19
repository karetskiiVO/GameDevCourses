using System.Collections.Generic;

namespace Life {

struct Fraction {}

abstract class Tile {
    public Fraction fraction{get; set;}
    private Fraction next;

    private Tile[] neightbours;

    // mesh
    
    public abstract void Forward();
    public abstract void Update();
}




}