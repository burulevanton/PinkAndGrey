using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using ObjectPool;
using Serialize;
using UnityEngine;

public class Collectable : TileController
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PoolManager.ReleaseObject(gameObject);
    }

    public override ISerializableTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileInfo
        {
            TileType = TileType.Collectable,
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
