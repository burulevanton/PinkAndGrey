using System.Collections;
using Enum;
using ObjectPool;
using Serialize;
using UnityEngine;
using UnityEngine.Serialization;

public class CannonController : TileController
{

    [SerializeField] private Vector2 _direction;

    [SerializeField] private GameObject _projectilePrefab;
    
    [SerializeField]
    private float _shootInterval = 2f;

    private float _timer;

    private GameObject _item = null;

    private Animator _animator;

    [SerializeField]private TileType tileType;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnEnable()
    {
        if (_item != null)
        {
            PoolManager.ReleaseObject(_item);
        }
        _timer = _shootInterval;
//        StartCoroutine(Shoot());
    }

    private void Update()
    {
        if (GameController.Instance.IsPaused)
            return;
        if (_timer >= 0.0f)
            _timer -= Time.deltaTime;
        else
        {
//            var position = transform.position + (Vector3)_direction * 0.5f;
//            _item = PoolManager.SpawnObject(_projectilePrefab, position, transform.rotation);
//            _item.GetComponent<ProjectileController>().Direction = _direction;
//            _timer = _shootInterval;
//            _item.transform.parent = transform;
              _animator.SetTrigger("Shoot");
              _timer = _shootInterval;
        }
    }

    private void Shoot()
    {
        var position = transform.position + (Vector3)_direction * 0.5f;
        _item = PoolManager.SpawnObject(_projectilePrefab, position, transform.rotation);
        _item.GetComponent<ProjectileController>().Direction = _direction;
        _item.transform.parent = transform;
    }


    public override StaticTileInfo Serialize()
    {
        var staticTileInfo = new StaticTileWithSomeDirectionInfo()
        {
            TileType = tileType,
            Position = transform.position,
            Rotation = transform.rotation.eulerAngles,
            Direction = _direction
        };
        return staticTileInfo;
    }

    public override bool Deserialize(StaticTileInfo tileInfo)
    {
        var info = tileInfo as StaticTileWithSomeDirectionInfo;
        if (info == null)
            return false;
        transform.position = info.Position;
        _direction = info.Direction;
        transform.rotation = Quaternion.Euler(info.Rotation);
        return true;
    }

//    protected override void OnUpdate()
//    {
//        if (_timer > 0.0f)
//        {
//            _timer -= Time.deltaTime;
//        }
//        else
//        {
//            
//        }
//    }
//
//    protected override void OnFixedUpdate()
//    {
//        throw new System.NotImplementedException();
//    }
}
