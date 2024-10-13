// TODO foreach
public class DynamicBuffer<T> {
    private int capacity = 2;
    private int count = 0;
    private T[] values = new T[2];

    public int Count{
        get => count;
    }

    public T this[int idx] {
        get => values[idx];
        set => values[idx] = value;
    }

    public void Add (T value) {
        if (count >= capacity) Reserve(2 * capacity);
        values[count++] = value;
    } 

    public void Reserve (int capacity) {
        if (this.capacity >= capacity) return;

        T[] newValues = new T[capacity];
        values.CopyTo(newValues, 0);
        values = newValues;

        this.capacity = capacity;
    }

    public void Clear () {
        count = 0;
    }

    public T[] ToArray () {
        // T[] res = new T[values.Length];
        // values.CopyTo(res, 0);
        // return res;
        return values;
    }
}

