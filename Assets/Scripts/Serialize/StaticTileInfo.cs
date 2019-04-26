using Enum;
using UnityEngine;

namespace Serialize
{
    public class StaticTileInfo
    {
        public TileType TileType { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }
}