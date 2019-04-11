using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using ObjectPool;
using Serialize;
using UnityEngine;

public class CopyingPortalController : TileController
{
    [SerializeField] public GameObject CopiedPlayerPrefab;

    public bool IsActivated = true;
//    [SerializeField] private MoveController _moveController;
//
//    private void OnTriggerExit2D(Collider2D other)
//    {
//        var other_movable = other.gameObject.GetComponent<Movable>();
//        CopiedPlayer.SetActive(true);
//        CopiedPlayer.transform.position = new Vector3(transform.position.x - other_movable.MovableDirection.Horizontal,
//            transform.position.y - other_movable.MovableDirection.Vertical, CopiedPlayer.transform.position.z);
//        _moveController.SetDirection(CopiedPlayer.gameObject.GetComponent<Movable>());
//        gameObject.SetActive(false);
//    }
    public void ActivatePortal(Vector2 moveDirection)
    {
        var copiedPlayerClone = PoolManager.SpawnObject(CopiedPlayerPrefab, new Vector3(transform.position.x - moveDirection.x,
        transform.position.y - moveDirection.y, 0.0f), Quaternion.identity);
        PoolManager.ReleaseObject(this.gameObject);
        IsActivated = false;
        var playerController = copiedPlayerClone.GetComponent<PlayerController>();
        playerController.JumpWithDirection(moveDirection);
    }

    public override StaticTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileInfo
        {
            TileType = TileType.CopyingPortal,
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z
        };
        return staticTileInfo;
    }

    public override bool Deserialize(StaticTileInfo tileInfo)
    {
        var info = tileInfo as StaticTileInfo;
        if (info == null)
            return false;    
        transform.position = new Vector3(info.X, info.Y, info.Z);
        return true;
    }
}
