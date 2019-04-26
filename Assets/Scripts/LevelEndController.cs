using Enum;
using Serialize;
using UnityEngine;

public class LevelEndController:TileController
{
    public override StaticTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileInfo
        {
        TileType = TileType.LevelEnd,
        Position = transform.position,
        Rotation = transform.rotation.eulerAngles
        };
        return staticTileInfo;
    }

    public override bool Deserialize(StaticTileInfo tileInfo)
    {
        transform.position = tileInfo.Position;
        transform.rotation = Quaternion.Euler(tileInfo.Rotation);
        return true;
    }
}