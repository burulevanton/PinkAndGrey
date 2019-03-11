using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using Serialize;
using UnityEngine;

public class GreaterSpikeTriggerController : TileController
{

    [SerializeField] private GameObject GreaterSpikeResultPrefab;

    private BoxCollider2D _boxCollider2D;

    [SerializeField] private Vector2 _direction;

    public Vector2 Direction => _direction;

    private void Start()
    {
        _boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _boxCollider2D.enabled = false;
        var item = (GameObject) Instantiate(GreaterSpikeResultPrefab);
        item.transform.parent = transform;
        item.transform.position = transform.position;
        var spikesResult = item.gameObject.GetComponent<GreaterSpikesResultController>();
        spikesResult.GreaterSpikeTriggerController = this;
    }

    public void EnableTrigger()
    {
        _boxCollider2D.enabled = true;
    }

    public override ISerializableTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileInfo
        {
            TileType = TileType.GreaterSpike,
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z
        };
        return staticTileInfo;
    }

    public override bool Deserialize(ISerializableTileInfo tileInfo)
    {
        throw new NotImplementedException();
    }
}
