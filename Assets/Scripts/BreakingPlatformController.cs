using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using ObjectPool;
using Serialize;
using UnityEngine;

public class BreakingPlatformController : TileController
{
    public void BreakPlatform()
    {
        PoolManager.ReleaseObject(gameObject);
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
        var info = tileInfo as StaticTileInfo;
        if (info == null)
            return false;    
        transform.position = new Vector3(info.X, info.Y, info.Z);
        return true;
    }
}
