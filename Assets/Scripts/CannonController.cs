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

    private GameObject _item = null;
    

    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(_shootInterval);
            if (_item == null || !_item.activeSelf)
            {
                Vector3 position = transform.position + (Vector3)_direction * 0.5f;
                _item = PoolManager.SpawnObject(_projectilePrefab, position, transform.rotation);
                _item.GetComponent<ProjectileController>().Direction = _direction;
            }
        }
    }

    public override ISerializableTileInfo Serialize()
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

    public override bool Deserialize(ISerializableTileInfo tileInfo)
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
}
