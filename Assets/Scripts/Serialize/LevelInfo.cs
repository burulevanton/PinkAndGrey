using System.Collections.Generic;
using Enum;
using UnityEngine;

namespace Serialize
{
    public class LevelInfo
    {
        public Dictionary<TileType,List<StaticTileInfo>> TileGameObjects { get; set; }

        public List<PaletteTileInfo> PaletteTileInfos { get; set; }

        public List<PaletteTileInfo> TileWalls { get; set; }
        public List<PaletteTileInfo> TileSpikes { get; set; }
        public Vector3 PlayerPos { get; set; }
    }
}