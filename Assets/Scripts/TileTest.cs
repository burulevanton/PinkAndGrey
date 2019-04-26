using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Serialize;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public Tilemap Tilemap;
    public List<TileBase> tiles;

    private List<PaletteTileInfo> _list = new List<PaletteTileInfo>();
    void Start () {
//        for (int i = Tilemap.cellBounds.xMin; i < Tilemap.cellBounds.xMax; i++)
//        {
//            for (int j = Tilemap.cellBounds.yMin; j < Tilemap.cellBounds.yMax; j++)
//            {
//                Vector3Int localPlace = (new Vector3Int(i, j, (int) Tilemap.transform.position.y));
//                if (Tilemap.HasTile(localPlace))
//                {
//                    var tile = Tilemap.GetTile(localPlace);
//                    var tileInfo = new PaletteTileInfo
//                    {
//                    X = i, Y = j, IndexOfArray = tiles.FindIndex(x => x.name == tile.name)
//                    };
//                    _list.Add(tileInfo);
//                }
//            }
//        }
//
//        var json = JsonConvert.SerializeObject(_list, Formatting.Indented);
//        string path = Application.dataPath + "/tiles.json";
//        Debug.Log(path);
//        File.WriteAllText(path, json, Encoding.UTF8);
//        string path = Application.dataPath + "/tiles.json";
//        var json = File.ReadAllText(path);
//        _list = JsonConvert.DeserializeObject<List<PaletteTileInfo>>(json);
//        foreach (var l in _list)
//        {
//            Tilemap.SetTile(new Vector3Int(l.X,l.Y, 0), tiles[l.IndexOfArray]);
//        }
    }         
}