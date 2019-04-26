using System.Collections;
using System.Collections.Generic;
using Enum;
using Serialize;
using UnityEngine;

public class TimerWallController : TileController
{

//    [SerializeField] private Sprite _deactivatedSprite;
//    [SerializeField] private Sprite _activatedSprite;

    private float _activeTime = 2.0f;

    private float _activeTimer;

    public bool IsActivated => _activeTimer > 0.0f;
    private int _landedPlayers = 0;

    private Animator _animator;
    
    public void ActivateWall()
    {
        this._activeTimer = _activeTime;
        _animator.SetTrigger("Activate");
    }

    // Start is called before the first frame update
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.IsPaused)
            return;
        if (this._activeTimer>0.0f)
        {
            this._activeTimer -= Time.deltaTime;
            if (this._activeTimer <= 0.0f)
            {
                if (_landedPlayers>0)
                {
                    this._activeTimer = 0.1f;
                }
                else
                {
                    _animator.SetTrigger("Deactivate");
                }
            }
        }
    }

    public void PlayerLand()
    {
        _landedPlayers++;
    }

    public void PlayerExit()
    {
        _landedPlayers = _landedPlayers > 0 ? _landedPlayers - 1 : 0;
    }

    public override StaticTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileInfo
        {
            TileType = TileType.TimerWall,
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
