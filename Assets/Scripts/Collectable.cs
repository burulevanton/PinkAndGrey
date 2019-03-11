using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using Serialize;
using UnityEngine;

public class Collectable : TileController
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
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
        throw new System.NotImplementedException();
    }
}
