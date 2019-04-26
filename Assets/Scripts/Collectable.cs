using System.Collections;
using System.Collections.Generic;
using Enum;
using ObjectPool;
using Serialize;
using UI;
using UnityEngine;

public class Collectable : TileController
{

    [SerializeField] private ScoreController _scoreController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PoolManager.ReleaseObject(gameObject);
            GameData.Instance.CurrentScoreOnLevel++;
            _scoreController.UpdateScore();
        }
    }

    public override StaticTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileInfo
        {
            TileType = TileType.Collectable,
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
