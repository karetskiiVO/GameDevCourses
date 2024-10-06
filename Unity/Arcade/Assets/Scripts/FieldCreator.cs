using System.Collections.Generic;
using UnityEngine;

public abstract class FieldCreator : MonoBehaviour {
    protected class MeshAccumulator {
        private List<Vector2> uv       = new List<Vector2>();
        private List<Vector3> vertices = new List<Vector3>();
        private List<int> triangles    = new List<int>();
        private Dictionary<Vector3, int> vertIndexes = new Dictionary<Vector3, int>();

        public Vector2[] UV () {
            return uv.ToArray();
        }
        public Vector3[] Vertices () {
            return vertices.ToArray();
        }
        public int[] Triangles () {
            return triangles.ToArray();
        }

        // Добавит грань, если часть вершин уже существует и имеют uv координаты, то новые будут проигнорированны
        public (int, int, int) Add((Vector3, Vector3, Vector3) edge, (Vector2, Vector2, Vector2) uvedge) {
            Vector3[] verts   = {  edge.Item1,   edge.Item2,   edge.Item3};
            Vector2[] uvVerts = {uvedge.Item1, uvedge.Item2, uvedge.Item3};
            int[] buf = {0, 0, 0};

            (int, int, int) res;
            for (var i = 0; i < 3; i++) {
                int newIdx;
                if (vertIndexes.ContainsKey(verts[i])) {
                    newIdx = vertIndexes[verts[i]];
                } else {
                    newIdx = vertices.Count;
                    
                    vertices.Add(verts[i]);
                    uv.Add(uvVerts[i]);

                    vertIndexes.Add(verts[i], newIdx);
                }
                triangles.Add(newIdx);

                buf[i] = newIdx;
            }

            res.Item1 = buf[0];
            res.Item2 = buf[1];
            res.Item3 = buf[2];

            return res;
        }
    }

    public abstract Tile[] CreateField (Field field);
}
