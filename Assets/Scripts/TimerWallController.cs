using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Enum;
using Serialize;
using UnityEngine;
using UnityEngine.WSA;

public class TimerWallController : TileController
{

    private SpriteRenderer _spriteRenderer;
    private PlayerController _playerController;
    private GameController _gameController;

    [SerializeField] private Sprite _deactivatedSprite;
    [SerializeField] private Sprite _activatedSprite;

    private float _activeTime = 2.0f;

    private float _activeTimer;

    public bool IsActivated => _activeTimer > 0.0f;

    public void ActivateWall()
    {
        this._activeTimer = _activeTime;
        this._spriteRenderer.sprite = _activatedSprite;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _gameController = GameController.Instance;
        _playerController = _gameController.PlayerController;
    }

    private void Reset()
    {
        _gameController = GameController.Instance;
        //_playerController = _gameController.PlayerController;
    }

    // Update is called once per frame
    void Update()
    {
        if (this._activeTimer>0.0f)
        {
            this._activeTimer -= Time.deltaTime;
            if (this._activeTimer <= 0.0f)
            {
                if (_gameController.PlayerController.OnTimerWall == this)
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

    public override ISerializableTileInfo Serialize()
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

    public override bool Deserialize(ISerializableTileInfo tileInfo)
    {
        throw new System.NotImplementedException();
    }
}
