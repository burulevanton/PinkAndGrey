using Enum;
using UnityEngine;

namespace Serialize
{
    public class DynamicTileInfo:StaticTileInfo
    {
        public Vector3 FromDirection { get; set; }
        public Vector3 ToDirection { get; set; }
    }
}