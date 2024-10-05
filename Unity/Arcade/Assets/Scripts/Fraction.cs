using UnityEngine;
    
public interface IFraction {
    public IFraction Interact (Tile tile);
    public int[] FlushUpdates ();
    public void AddUpdates(int upd);
}
