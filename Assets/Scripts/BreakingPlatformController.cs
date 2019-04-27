using System.Collections;
using System.Collections.Generic;
using Enum;
using ObjectPool;
using Serialize;
using UnityEngine;

public class BreakingPlatformController : TileController
{

    private Animator _animator;
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        _boxCollider2D.enabled = true;
    }

    public void BreakPlatform()
    {
        //PoolManager.ReleaseObject(gameObject);
        _animator.SetTrigger("Break");
        _boxCollider2D.enabled = false;
    }

    public override StaticTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileInfo
        {
            TileType = TileType.BreakingPlatform,
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
