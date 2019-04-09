using System.Collections.Generic;
using UnityEngine;

namespace Serialize
{
    public class LevelInfo
    {
        public List<ISerializableTileInfo> TileInfos { get; set; }
        public Vector3 PlayerPos { get; set; }
    }
}