using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using Serialize;
using UnityEngine;

public class TimerWallController : TileController
{

    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Sprite _deactivatedSprite;
    [SerializeField] private Sprite _activatedSprite;

    private float _activeTime = 2.0f;

    private float _activeTimer;

    public bool IsActivated => _activeTimer > 0.0f;
    private int _landedPlayers = 0;

    public void ActivateWall()
    {
        this._activeTimer = _activeTime;
        this._spriteRenderer.sprite = _activatedSprite;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
                    _spriteRenderer.sprite = _deactivatedSprite;
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
