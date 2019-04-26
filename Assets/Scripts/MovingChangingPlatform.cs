using System.Collections;
using System.Collections.Generic;
using Enum;
using Serialize;
using UnityEngine;

public class MovingChangingPlatform : TileController
{
    [SerializeField]
    private Vector2 _direction = Vector2.one;

    public Vector2 Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    public override StaticTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileWithSomeDirectionInfo()
        {
            TileType = TileType.MovingChangingPlatform,
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles,
            Direction = _direction
        };
        return staticTileInfo;
    }

    public override bool Deserialize(StaticTileInfo tileInfo)
    {
        var info = tileInfo as StaticTileWithSomeDirectionInfo;
        if (info == null)
            return false;
        transform.position = info.Position;
        transform.rotation = Quaternion.Euler(info.Rotation);
        _direction = info.Direction;
        return true;
    }
}
