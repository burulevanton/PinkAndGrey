using System.Collections;
using System.Collections.Generic;
using Enum;
using Serialize;
using UnityEngine;

public class PortalController: TileController
{

    public GameObject OtherPortal;


//    private void OnTriggerEnter2D(Collider2D other)
    //    {
    //        if (other.gameObject.CompareTag("Player"))
    //        {
    //            Movable movable = other.gameObject.GetComponent<Movable>();
    //           other.gameObject.transform.position = movable.MovableDirection.GetOffsetAfterPortal(OtherPortal.transform.position);
    //        }
    //    }

    public override StaticTileInfo Serialize()
    {
        var portalTileInfo = new PortalTileInfo
        {
            TileType = TileType.Portal,
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles,
            OtherPortalPosition = OtherPortal.transform.position
        };
        return portalTileInfo;
    }

    public override bool Deserialize(StaticTileInfo tileInfo)
    {
        var info = tileInfo as PortalTileInfo;
        if (info == null)
            return false;
        transform.position = info.Position;
        transform.rotation = Quaternion.Euler(info.Rotation);
        return true;
    }
}
