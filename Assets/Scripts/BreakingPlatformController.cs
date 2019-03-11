using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using Serialize;
using UnityEngine;

public class BreakingPlatformController : TileController
{
    public void BreakPlatform()
    {
        gameObject.SetActive(false);
    }

    public override ISerializableTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileInfo
        {
            TileType = TileType.BreakingPlatform,
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
