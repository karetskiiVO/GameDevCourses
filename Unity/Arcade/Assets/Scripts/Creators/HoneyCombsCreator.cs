using System;
using System.Collections.Generic;
using UnityEngine;

public class HoneyCombsCreator : FieldCreator {
    [SerializeField]int radius = 12;

    protected override FieldCameraController ActualController () {
        return new PlaneCameraController(Vector3.forward);
    }

    protected override (List<Tile>, MeshAccumulator) LogicalCreateField (Field field) {
        var buf = new List<Tile>();
        
        var meshAccumulator = new MeshAccumulator();        
        var tiles = new Dictionary<Vector2Int, Tile>();

        Vector3 e1 = Quaternion.AngleAxis(-60, Vector3.forward) * Vector3.right;
        Vector3 e2 = Vector3.right;

        var border = radius;
        // Создаем бессвязное поле и Mesh под него
        for (var x = -border; x <= border; x++) {
            for (var y = -border; y <= border; y++) {
                var position = new Vector2Int(x, y);
                var center = x * (e1 + e2) + y * (2 * e2 - e1);

                if (Math.Abs(x + y) > radius) continue;

                var newTile = meshAccumulator.NewTile(new MeshAccumulator.TileEdge[] {
                    new MeshAccumulator.TileEdge(
                        edge:   (center + e1, center - e2, center - e1 + e2),
                        uvedge: (position + Vector2.zero, position + Vector2.up, position + Vector2.right)
                    ),
                    new MeshAccumulator.TileEdge(
                        edge:   (center + e1 - e2, center - e2, center + e1),
                        uvedge: (position + Vector2.zero, position + Vector2.up, position + Vector2.right)
                    ),
                    new MeshAccumulator.TileEdge(
                        edge:   (center - e1, center - e1 + e2, center - e2),
                        uvedge: (position + Vector2.zero, position + Vector2.up, position + Vector2.right)
                    ),
                    new MeshAccumulator.TileEdge(
                        edge:   (center + e2, center + e1, center - e1 + e2),
                        uvedge: (position + Vector2.zero, position + Vector2.up, position + Vector2.right)
                    ),
                });

                tiles.Add(position, newTile);
                buf.Add(newTile);
            }
        }

        // Настраиваем связи
        Vector2Int[] directions = {
            Vector2Int.right, Vector2Int.right + Vector2Int.down, Vector2Int.down,
            Vector2Int.left , Vector2Int.left  + Vector2Int.up  , Vector2Int.up  ,
        };
        var neighboursList = new List<Tile>();
        for (var x = -border; x <= border; x++) {
            for (var y = -border; y <= border; y++) {
                var position = new Vector2Int(x, y);
                
                if (!tiles.ContainsKey(position)) continue;
                var tile = tiles[position];

                // Собираем всех соседей
                neighboursList.Clear();
                foreach (var direction in directions) {
                    var bufPosition = position + direction;

                    if (!tiles.ContainsKey(bufPosition)) continue;
                    neighboursList.Add(tiles[bufPosition]);
                }

                tile.neightbours = neighboursList.ToArray();                    
            }
        }
        
        return (buf, meshAccumulator);
    }
}
