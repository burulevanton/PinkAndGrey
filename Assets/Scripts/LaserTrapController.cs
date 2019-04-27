using System.Diagnostics;
using Enum;
using Serialize;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LaserTrapController : TileController
{

    [SerializeField] private GameObject laserTrapTrigger;
    [SerializeField] private GameObject laserTrapResult;

    private BoxCollider2D _triggerCollider;
    private BoxCollider2D _resultCollider;

    private bool _isTriggerActivated = false;
    private bool _isAttacked = false;
    [SerializeField] private float _preAttackTime = 0.5f;
    [SerializeField] private float _attackTime = 1f;
    private float _timer;

    private Animator _animator;

    private void Awake()
    {
        _triggerCollider = laserTrapTrigger.GetComponent<BoxCollider2D>();
        _resultCollider = laserTrapResult.GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _isTriggerActivated = false;
        _isAttacked = false;
        _timer = 0;
    }

    private void Update()
    {
        if (GameController.Instance.IsPaused)
            return;
        if (_isTriggerActivated)
        {
            if (_timer > 0.0f)
                _timer -= Time.deltaTime;
            else
            {
                if (_isAttacked)
                {
                    _isAttacked = false;
                    _isTriggerActivated = false;
                    _triggerCollider.enabled = true;
                    _resultCollider.enabled = false;
                    laserTrapResult.SetActive(false);
                    _animator.SetTrigger("LaserDeactivate");
                }
                else
                {
                    _isAttacked = true;
                    _resultCollider.enabled = true;
                    _timer = _attackTime;
                    _animator.SetTrigger("LaserActivate");
                }
            }
        }
    }

    public void ActivateLaserTrap()
    {
        _timer = _preAttackTime;
        _isTriggerActivated = true;
        _triggerCollider.enabled = false;
        laserTrapResult.SetActive(true);
        _animator.SetTrigger("TriggerActivate");
    }

    public override StaticTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileInfo()
        {
            TileType = TileType.GreaterSpike,
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