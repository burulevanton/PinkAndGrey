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

    public new ISerializableTileInfo Serialize()
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

    public new bool Deserialize(ISerializableTileInfo tileInfo)
    {
        throw new System.NotImplementedException();
    }
}
