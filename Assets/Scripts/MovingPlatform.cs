using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Enum;
using Serialize;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MovingPlatform : TileController
{
    [SerializeField] protected Vector3 FromDirection;
    [SerializeField] protected Vector3 ToDirection;
    [SerializeField] private float moveSpeed;

    [SerializeField] private bool reverseMove = false;
    [SerializeField] protected bool updateRotation;
    private float _timer;
    [SerializeField] private float _stopTime = 0.5f;
    private float _journeyLength;
    private float _distCovered;
    private float _fracJourney;
    private bool _isPause = false;

    protected void OnReset()
    {
        _distCovered = 0;
        transform.position = FromDirection;
        _timer = _stopTime;
        _isPause = true;
        reverseMove = true;
        _journeyLength = Vector3.Distance(FromDirection, ToDirection);
    }

    private void OnDisable()
    {
        _isPause = true;
    }

    void Update()
    {
        if (GameController.Instance.IsPaused)
            return;
        if (_isPause)
        {
            if (_timer > 0.0f)
                _timer -= Time.deltaTime;
            else
            {
                _isPause = false;
                _timer = 0.0f;
                reverseMove = !reverseMove;
            }
            return;
        }
        _distCovered += Time.deltaTime * moveSpeed;        
        _fracJourney = _distCovered / _journeyLength;  
        if ((Vector3.Distance(transform.position, ToDirection) == 0.0f && !reverseMove) ||
            (Vector3.Distance(transform.position, FromDirection) == 0.0f) && reverseMove)
        {
            if (updateRotation)
                UpdateRotation();
            _isPause = true;
            _timer = _stopTime;
            _distCovered = 0;
            return;
        }

        if (!reverseMove)
        {
            transform.position = Vector3.Lerp(FromDirection, ToDirection, _fracJourney);
        }
        else
        {
            transform.position = Vector3.Lerp(ToDirection, FromDirection, _fracJourney);
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

    public bool IsEndOfJourneyPlayer(Vector3 hit)
    {
        if (this.Direction == Vector2.up)
        {
            var vector = ToDirection + new Vector3(-1, 0.5f);
            var heh = (hit - vector).sqrMagnitude;
            if ((hit - (ToDirection + new Vector3(-1, 0.5f))).sqrMagnitude < 0.0001f || (hit - (ToDirection + new Vector3(1, 0.5f))).sqrMagnitude < 0.0001f)
                return true;
            return false;
        }
        if (this.Direction == Vector2.down)
        {
            if ((hit - (FromDirection + new Vector3(-1, -0.5f))).sqrMagnitude < 0.0001f || (hit - (FromDirection + new Vector3(1, -0.5f))).sqrMagnitude < 0.0001f)
            {
                return true;
            }
            return false;
        }
        if (this.Direction == Vector2.right)
        {
            if (hit == ToDirection + new Vector3(0.5f, -1) || hit == ToDirection + new Vector3(0.5f, 1))
                return true;
            return false;
        }
        if (this.Direction == Vector2.left)
        {
            if (hit == FromDirection + new Vector3(-0.5f, 1) || hit == FromDirection + new Vector3(-0.5f, -1))
            {
                return true;
            }
            return false;
        }
        return false;
    }

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
        transform.rotation = Quaternion.Euler(info.Rotation);
        FromDirection = info.FromDirection;
        ToDirection = info.ToDirection;
        OnReset();
        return true;
    }
}
