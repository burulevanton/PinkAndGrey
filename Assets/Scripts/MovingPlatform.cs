using System;
using System.Collections;
using System.Collections.Generic;
using Enum;
using Serialize;
using UnityEngine;

public class MovingPlatform : TileController
{
    [SerializeField] protected Vector3 FromDirection;
    [SerializeField] protected Vector3 ToDirection;
    [SerializeField] private float moveSpeed;

    [SerializeField] private bool reverseMove = false;
    [SerializeField] protected bool updateRotation;
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
        if (GameController.Instance.IsPaused)
            return;
        if (_isPause)
        {
            if (Time.time - _startTime > 0.5f)
            {
                _isPause = false;
                _startTime = Time.time;
                reverseMove = !reverseMove;
            }
            return;
        }
        _distCovered = (Time.time - _startTime) * moveSpeed;
        Debug.Log("DistCovered:" + _distCovered + "MoveSpeed:" +moveSpeed * Time.deltaTime + "JourneyLength" + _journeyLength);
        _fracJourney = _distCovered / _journeyLength;
        Debug.Log(_fracJourney);
        if (!reverseMove)
        {
            transform.position = Vector3.Lerp(FromDirection, ToDirection, _fracJourney);
        }
        else
        {
            transform.position = Vector3.Lerp(ToDirection, FromDirection, _fracJourney);
        }
        if ((Vector3.Distance(transform.position, ToDirection) == 0.0f || Vector3.Distance(transform.position, FromDirection) == 0.0f))
        {
            if (updateRotation)
                UpdateRotation();
            _isPause = true;
            _startTime = Time.time;
        }
    }

    public Vector2 Direction
    {
        get
        {
            if (FromDirection.x == ToDirection.x)
            {
                return reverseMove ? Vector2.down  : Vector2.up;
            }
            else
            {
                return reverseMove ? Vector2.left: Vector2.right;
            }
        }
    }

    public float MoveSpeed => moveSpeed;

    public bool IsPause => _isPause;

    public override StaticTileInfo Serialize()
    {
        var dynamicTileInfo = new DynamicTileInfo()
        {
            TileType = TileType.MovingPlatform,
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles,
            FromDirection = FromDirection,
            ToDirection = ToDirection
        };
        return dynamicTileInfo;
    }

    private void UpdateRotation()
    {
        transform.rotation = reverseMove ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
    }

    public override bool Deserialize(StaticTileInfo tileInfo)
    {
        var info = tileInfo as DynamicTileInfo;
        if (info == null)
            return false;
        transform.position = info.Position;
        transform.rotation = Quaternion.Euler(info.Rotation);
        FromDirection = info.FromDirection;
        ToDirection = info.ToDirection;
        return true;
    }
}
