using Enum;
using Serialize;
using UnityEngine;

namespace DefaultNamespace
{
    public class InnerWallController : TileController
    {
        public override ISerializableTileInfo Serialize()
        {
            var staticTileInfo = new StaticTileInfo
            {
                TileType = TileType.InnerWall,
                X = transform.position.x,
                Y = transform.position.y,
                Z = transform.position.z
            };
            return staticTileInfo;
        }

        public override bool Deserialize(ISerializableTileInfo tileInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}