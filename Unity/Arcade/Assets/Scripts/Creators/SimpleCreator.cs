using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Rendering;

public class SimpleCreator : FieldCreator {
    [SerializeField]Vector2Int size = new Vector2Int(12, 12);

    public override Tile[] CreateField(Field field) {
        var buf = new List<Tile>();
        
        var meshAccumulator = new MeshAccumulator();        
        var tiles = new Dictionary<Vector2Int, Tile>();

        // Создаем бессвязное поле и Mesh под него
        for (var x = 0; x < size.x; x++) {
            for (var y = 0; y < size.y; y++) {
                var position = new Vector2Int(x, y);
                var triangleStartPoint = new Vector3(x - size.x/2, y - size.y/2, 0);

                var tr1 = meshAccumulator.Add(
                    edge:   (triangleStartPoint, triangleStartPoint + Vector3.up, triangleStartPoint + Vector3.right),
                    uvedge: (position + Vector2.zero, position + Vector2.up, position + Vector2.right)
                );
                var tr2 = meshAccumulator.Add(
                    edge:   (triangleStartPoint + Vector3.up + Vector3.right, triangleStartPoint + Vector3.right, triangleStartPoint + Vector3.up),
                    uvedge: (position + Vector2.up + Vector2.right, position + Vector2.right, position + Vector2.up)
                );
                int[] idxes = new int[] {
                    tr1.Item1, tr1.Item2, tr1.Item3,
                    tr2.Item1, tr2.Item2, tr2.Item3,
                };

                var newTile = new Tile(idxes);
                tiles.Add(position, newTile);
                buf.Add(newTile);
            }
        }

        if (field.GetComponent<MeshFilter>().mesh == null) field.GetComponent<MeshFilter>().mesh = new Mesh();
        var mesh = field.GetComponent<MeshFilter>().mesh;
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

        var res = new Tile[buf.Count];
        buf.ToArray().CopyTo(res, 0);
        
        return res;
    }
}
