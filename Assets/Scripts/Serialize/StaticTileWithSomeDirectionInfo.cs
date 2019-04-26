using Enum;
using UnityEngine;

namespace Serialize
{
    public class StaticTileWithSomeDirectionInfo: StaticTileInfo
    {
        public Vector3 Direction { get; set; }
    }
}