using DefaultNamespace;
using Enum;
using Serialize;
using UnityEngine;

public class LaserTrap : TileController
{
    public override StaticTileInfo Serialize()
    {
        var greaterSpikeTrigger = gameObject.GetComponentInChildren<GreaterSpikeTriggerController>();
        var staticTileInfo = new StaticTileWithSomeDirectionInfo()
        {
            TileType = TileType.GreaterSpike,
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z,
            DirectionX = greaterSpikeTrigger.Direction.x,
            DirectionY = greaterSpikeTrigger.Direction.y,
            RotationZ = transform.eulerAngles.z
        };
        return staticTileInfo;
    }

    public override bool Deserialize(StaticTileInfo tileInfo)
    {
        var info = tileInfo as StaticTileWithSomeDirectionInfo;
        if (info == null)
            return false;
        transform.position = new Vector3(info.X, info.Y, info.Z);
        var greaterSpikeTrigger = gameObject.GetComponentInChildren<GreaterSpikeTriggerController>();
        greaterSpikeTrigger.Direction = new Vector2(info.DirectionX, info.DirectionY);
        transform.rotation = Quaternion.Euler(0f, 0f, info.RotationZ);
        return true;
    }
}