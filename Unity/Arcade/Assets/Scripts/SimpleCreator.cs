using System;
using System.Collections;
using System.Collections.Generic;
using Life;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleCreator : MonoBehaviour, IFieldCreator {
    private class MeshAccumulator {
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
        public void Add((Vector3, Vector3, Vector3) edge, (Vector2, Vector2, Vector2) uvedge) {
            Vector3[] verts   = {  edge.Item1,   edge.Item2,   edge.Item3};
            Vector2[] uvVerts = {uvedge.Item1, uvedge.Item2, uvedge.Item3};

            for (var i = 0; i < 3; i++) {
                if (vertIndexes.ContainsKey(verts[i])) {
                    triangles.Add(vertIndexes[verts[i]]);
                } else {
                    var newIdx = vertices.Count;
                    
                    vertices.Add(verts[i]);
                    uv.Add(uvVerts[i]);

                    vertIndexes.Add(verts[i], newIdx);

                    triangles.Add(newIdx);
                }
            }
        }
    }

    [SerializeField]Vector2Int size = new Vector2Int(12, 12);

    public Tile[] CreateField(Field field) {
        var res = new List<Tile>();
        
        var meshAccumulator = new MeshAccumulator();        
        var tiles = new Dictionary<Vector2Int, Tile>();

        // Создаем бессвязное поле и Mesh под него
        for (var x = 0; x < size.x; x++) {
            for (var y = 0; y < size.y; y++) {
                var position = new Vector2Int(x, y);
                var triangleStartPoint = new Vector3(x, y, 0);

                // TODO: связать вершину с гранью
                meshAccumulator.Add(
                    edge:   (triangleStartPoint, triangleStartPoint + Vector3.up, triangleStartPoint + Vector3.right),
                    uvedge: (position + Vector2.zero, position + Vector2.up, position + Vector2.right)
                );
                meshAccumulator.Add(
                    edge:   (triangleStartPoint + Vector3.up + Vector3.right, triangleStartPoint + Vector3.right, triangleStartPoint + Vector3.up),
                    uvedge: (position + Vector2.up + Vector2.right, position + Vector2.right, position + Vector2.up)
                );

                var newTile = new Tile();
                tiles.Add(position, newTile);
                res.Add(newTile);
            }
        }

        var mesh = field.GetComponent<MeshFilter>().sharedMesh;
        mesh.Clear();

        mesh.vertices  = meshAccumulator.Vertices();
        mesh.uv        = meshAccumulator.UV();
        mesh.triangles = meshAccumulator.Triangles();

        // Настраиваем связи
        Vector2Int[] directions = {
            Vector2Int.left + Vector2Int.up  , Vector2Int.up  , Vector2Int.up   + Vector2Int.right,
            Vector2Int.left                  ,                                    Vector2Int.right,
            Vector2Int.left + Vector2Int.down, Vector2Int.down, Vector2Int.down + Vector2Int.right
        };
        var neighboursList = new List<Tile>();
        for (var x = 0; x < size.x; x++) {
            for (var y = 0; y < size.y; y++) {
                var position = new Vector2Int(x, y);
                var tile = tiles[position];

                // Собираем всех соседей
                neighboursList.Clear();
                foreach (var direction in directions) {
                    var bufPosition = position + direction;

                    if (bufPosition.x < 0 || bufPosition.x >= size.x ||
                        bufPosition.y < 0 || bufPosition.y >= size.y) continue;
                
                    neighboursList.Add(tiles[bufPosition]);
                }

                tile.neightbours = neighboursList.ToArray();                    
            }
        }

        return res.ToArray();
    }
}
