using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using Serialize;
using UnityEngine;
using UnityEngine.WSA;

public class MovingPlatform : TileController
{
    [SerializeField] protected Vector3 FromDirection;
    [SerializeField] protected Vector3 ToDirection;
    [SerializeField] private float moveSpeed;

    [SerializeField] private bool reverseMove = false;
    private float _startTime;
    private float _journeyLength;
    private float _distCovered;
    private float _fracJourney;
    private bool _isPause = false;
    void Start()
    {
        _startTime = Time.time;
        _journeyLength = Vector3.Distance(FromDirection, ToDirection);
    }
    void Update()
    {
                
        if (_isPause)
        {
            if (Time.time - _startTime > 0.5f)
            {
                _isPause = false;
                _startTime = Time.time;
            }
            return;
        }
        _distCovered = (Time.time - _startTime) * moveSpeed;
        _fracJourney = _distCovered / _journeyLength;
        if (reverseMove)
        {
            transform.position = Vector3.Lerp(FromDirection, ToDirection, _fracJourney);
        }
        else
        {
            transform.position = Vector3.Lerp(ToDirection, FromDirection, _fracJourney);
        }
        if ((Vector3.Distance(transform.position, ToDirection) == 0.0f || Vector3.Distance(transform.position, FromDirection) == 0.0f))
        {

            reverseMove = !reverseMove;
            _isPause = true;
            _startTime = Time.time;
        }
    }

    public override ISerializableTileInfo Serialize()
    {
        var dynamicTileInfo = new DynamicTileInfo()
        {
            TileType = TileType.MovingPlatform,
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z,
            FromDirectionX = FromDirection.x,
            FromDirectionY = FromDirection.y,
            ToDirectionX = ToDirection.x,
            ToDirectionY = ToDirection.y
        };
        return dynamicTileInfo;
    }

    public override bool Deserialize(ISerializableTileInfo tileInfo)
    {
        throw new NotImplementedException();
    }
}
