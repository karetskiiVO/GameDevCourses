using UnityEngine;
    
public abstract class Fraction : MonoBehaviour {
    public DynamicBuffer<int> updates = new DynamicBuffer<int>();
    public Field field = null;
    public abstract Fraction Interact (Tile tile);

    public virtual void Init (Field field) {
        this.field = field;
    }

    public virtual int[] FlushUpdates () {
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
