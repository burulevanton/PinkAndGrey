using System.Collections;
using System.Collections.Generic;
using Enum;
using Serialize;
using UnityEngine;

public class EnemyController : MovingPlatform
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }

    public override StaticTileInfo Serialize()
    {
        var dynamicTileInfo = new DynamicTileInfo()
        {
            TileType = TileType.Enemy,
            Position = transform.position,
            FromDirection = FromDirection,
            ToDirection = ToDirection,
            Rotation = transform.rotation.eulerAngles
        };
        return dynamicTileInfo;
    }

    public override bool Deserialize(StaticTileInfo tileInfo)
    {
        var info = tileInfo as DynamicTileInfo;
        if (info == null)
            return false;
        transform.position = info.Position;
        FromDirection = info.FromDirection;
        ToDirection = info.ToDirection;
        transform.rotation = Quaternion.Euler(info.Rotation);
        return true;
    }
}
