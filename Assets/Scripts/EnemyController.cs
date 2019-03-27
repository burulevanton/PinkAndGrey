using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using Serialize;
using UnityEngine;

public class EnemyController : MovingPlatform
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }

    public override ISerializableTileInfo Serialize()
    {
        var dynamicTileInfo = new DynamicTileInfo()
        {
            TileType = TileType.Enemy,
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z,
            FromDirectionX = FromDirection.x,
            FromDirectionY = FromDirection.y,
            ToDirectionX = ToDirection.x,
            ToDirectionY = ToDirection.y
        };
        return dynamicTileInfo;
    }

    public override bool Deserialize(ISerializableTileInfo tileInfo)
    {
        var info = tileInfo as DynamicTileInfo;
        if (info == null)
            return false;
        transform.position = new Vector3(info.X, info.Y, info.Z);
        FromDirection = new Vector3(info.FromDirectionX, info.FromDirectionY);
        ToDirection = new Vector3(info.ToDirectionX, info.ToDirectionY);
        return true;
    }
}
