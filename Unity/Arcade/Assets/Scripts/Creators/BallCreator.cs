using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCreator : FieldCreator {
    [SerializeField]float radius     = 40;
    [SerializeField]int   layersCnt  = 10;
    [SerializeField]int   layersSize = 10;

    public override Tile[] CreateField(Field field) {
        var buf = new List<Tile>();
        
        var meshAccumulator = new MeshAccumulator();        
        var tiles = new Dictionary<Vector2Int, Tile>();

        float hDeltaAng = 2 * Mathf.PI / layersCnt;
        float vDeltaAng = Mathf.PI / (layersSize + 2);
        
        Vector3 createPoint(float hAng, float vAng) {
            var distance = radius * Mathf.Cos(vAng + vDeltaAng - Mathf.PI / 2);
            return new Vector3(
                distance * Mathf.Cos(hAng),
                radius   * Mathf.Sin(vAng + vDeltaAng - Mathf.PI / 2),
                distance * Mathf.Sin(hAng)
            );
        }
        
        // Создаем бессвязное поле и Mesh под него
        for (var x = 0; x < layersCnt; x++) {
            for (var y = 0; y < layersSize; y++) {
                var position = new Vector2Int(x, y);
                var tr1 = meshAccumulator.Add(
                    edge:   (   
                        createPoint((x + 1) * hDeltaAng, y * vDeltaAng),
                        createPoint(x * hDeltaAng, y * vDeltaAng), 
                        createPoint(x * hDeltaAng, (y + 1)* vDeltaAng)
                    ),
                    uvedge: (position + Vector2.zero, position + Vector2.up, position + Vector2.right)
                );
                var tr2 = meshAccumulator.Add(
                    edge:   (   
                        createPoint((x + 1) * hDeltaAng, (y + 1) * vDeltaAng),
                        createPoint((x + 1) * hDeltaAng, y * vDeltaAng), 
                        createPoint(x * hDeltaAng, (y + 1)* vDeltaAng)
                    ),
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

        // отдельно крышки
        // // верхняя
        // var topTr = new List<int>();
        // var botTr = new List<int>();
        // for (var x = 0; x < layersCnt; x++) {
        //     var position = new Vector2Int(x, layersSize - 1);
        //     var topTr = meshAccumulator.Add(
        //         edge:   (   
        //             createPoint((x + 1) * hDeltaAng, y * vDeltaAng),
        //             createPoint(x * hDeltaAng, y * vDeltaAng), 
        //             createPoint(x * hDeltaAng, (y + 1)* vDeltaAng)
        //         ),
        //         uvedge: (position + Vector2.zero, position + Vector2.up, position + Vector2.right)
        //     )
            
        //     //topTr.Add()


        // }

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
        for (var x = 0; x < layersCnt; x++) {
            for (var y = 0; y < layersSize; y++) {
                var position = new Vector2Int(x, y);
                var tile = tiles[position];

                // Собираем всех соседей
                neighboursList.Clear();
                foreach (var direction in directions) {
                    var bufPosition = position + direction;

                    if (0 <= bufPosition.y && bufPosition.y < layersSize) {
                        bufPosition.x = (bufPosition.x + layersCnt) % layersCnt;

                        neighboursList.Add(tiles[bufPosition]);
                    }
                }

                tile.neightbours = neighboursList.ToArray();                    
            }
        }

        var res = new Tile[buf.Count];
        buf.ToArray().CopyTo(res, 0);
        
        return res;
    }
}

