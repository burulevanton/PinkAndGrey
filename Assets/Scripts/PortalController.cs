using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
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

    public override ISerializableTileInfo Serialize()
    {
        var portalTileInfo = new PortalTileInfo
        {
            TileType = TileType.Portal,
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z,
            OtherPortalX = OtherPortal.transform.position.x,
            OtherPortalY = OtherPortal.transform.position.y,
            OtherPortalZ = OtherPortal.transform.position.z
        };
        return portalTileInfo;
    }

    public override bool Deserialize(ISerializableTileInfo tileInfo)
    {
        throw new System.NotImplementedException();
    }
}
