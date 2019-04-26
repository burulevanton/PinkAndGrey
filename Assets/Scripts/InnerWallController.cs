using System.Collections;
using System.Collections.Generic;
using Enum;
using Serialize;
using UnityEngine;

public class InnerWallController : TileController
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Sprite[] sprites;
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    public override StaticTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileInfo
        {
        TileType = TileType.InnerWall,
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
