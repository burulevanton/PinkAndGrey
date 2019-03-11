using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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

    public override ISerializableTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileWithSomeDirectionInfo()
        {
            TileType = TileType.MovingChangingPlatform,
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z,
            DirectionX = _direction.x,
            DirectionY = _direction.y
        };
        return staticTileInfo;
    }

    public override bool Deserialize(ISerializableTileInfo tileInfo)
    {
        throw new System.NotImplementedException();
    }
}
