using System.Collections.Generic;
using Enum;
using UnityEngine;

namespace Serialize
{
    public class LevelInfo
    {
        public Dictionary<TileType,List<ISerializableTileInfo>> TileInfos { get; set; }
        public Vector3 PlayerPos { get; set; }
    }
}