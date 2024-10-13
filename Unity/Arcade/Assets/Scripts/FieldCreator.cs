using System.Collections.Generic;
using UnityEngine;

public abstract class FieldCreator : MonoBehaviour {
    protected class MeshAccumulator {
        private readonly List<Vector2> uv            = new List<Vector2>();
        private readonly List<Vector3> vertices      = new List<Vector3>();
        private readonly List<int>     triangles     = new List<int>();
        private readonly List<Tile>    polygonToTile = new List<Tile>();

        private Dictionary<Vector3, int> vertIndexes = new Dictionary<Vector3, int>();

        public Vector2[] UV            () {
            return uv.ToArray();
        }
        public Vector3[] Vertices      () {
            return vertices.ToArray();
        }
        public int[]     Triangles     () {
            return triangles.ToArray();
        }
        public Tile[]    PolygonToTile () {
            return polygonToTile.ToArray();
        }

        public struct TileEdge {
            public (Vector3, Vector3, Vector3) edge;
            public (Vector2, Vector2, Vector2) uvedge;

            public TileEdge ((Vector3, Vector3, Vector3) edge, (Vector2, Vector2, Vector2) uvedge) {
                this.edge   = edge;
                this.uvedge = edge;
            }
        }

        // Добавит грань, если часть вершин уже существует и имеют uv координаты, то новые будут проигнорированны
        private (int, int, int) Add (TileEdge titeEdge) {
            var edge   = titeEdge.edge;
            var uvedge = titeEdge.uvedge;
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
    
        public Tile NewTile (TileEdge[] tileEdges) {
            var idxes = new List<int>();

            foreach (var tileEdge in tileEdges) {
                var additionalIdexes = Add(tileEdge);

                idxes.Add(additionalIdexes.Item1);
                idxes.Add(additionalIdexes.Item2);
                idxes.Add(additionalIdexes.Item3);
            }

            var res = new Tile(idxes);

            for (int i = 0; i < tileEdges.Length; i++) {
                polygonToTile.Add(res);
            }
            
            return res;
        }
    }

    protected abstract (List<Tile>, MeshAccumulator) LogicalCreateField (Field field);
    protected abstract FieldCameraController ActualController ();
    public void CreateField (Field field) {
        var res = LogicalCreateField(field);
        var meshAccumulator = res.Item2;
        field.map = res.Item1.ToArray();

        if (field.GetComponent<MeshFilter>().mesh == null) field.GetComponent<MeshFilter>().mesh = new Mesh();
        var mesh = field.GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        mesh.vertices  = meshAccumulator.Vertices();
        mesh.uv        = meshAccumulator.UV();
        mesh.triangles = meshAccumulator.Triangles();

        field.tilesFromEdgeIdeces = meshAccumulator.PolygonToTile();
        field.controller = ActualController();
    }
}
