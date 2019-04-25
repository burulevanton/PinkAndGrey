using System.Collections;
using DefaultNamespace;
using Enum;
using ObjectPool;
using Serialize;
using UnityEngine;

public class CannonController : TileController
{

    [SerializeField] private Vector2 _direction;

    [SerializeField] private GameObject _projectilePrefab;
    
    [SerializeField]
    private float _shootInterval = 2f;

    private float _timer;

    private GameObject _item = null;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Start()
    {
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
            TileType = TileType.Cannon,
            X = transform.position.x,
            Y = transform.position.y,
            Z = transform.position.z,
            DirectionX = _direction.x,
            DirectionY = _direction.y,
            RotationZ = transform.eulerAngles.z
        };
        return staticTileInfo;
    }

    public override bool Deserialize(StaticTileInfo tileInfo)
    {
        var info = tileInfo as StaticTileWithSomeDirectionInfo;
        if (info == null)
            return false;    
        transform.position = new Vector3(info.X, info.Y, info.Z);
        _direction.x = info.DirectionX;
        _direction.y = info.DirectionY;
        transform.rotation = Quaternion.Euler(0f, 0f, info.RotationZ);
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
