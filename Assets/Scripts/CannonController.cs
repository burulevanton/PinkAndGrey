using System.Collections;
using DefaultNamespace;
using Enum;
using Serialize;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.WSA;

public class CannonController : TileController
{

    [SerializeField] private Vector2 _direction;

    [SerializeField] private GameObject _projectilePrefab;
    
    [SerializeField]
    private float _shootInterval = 2f;

    private GameObject _item = null;
     

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(_shootInterval);
            if (_item == null || _item.gameObject == null)
            {
                Vector3 position = transform.position + (Vector3)_direction * 0.5f;
                _item = Instantiate(_projectilePrefab, position, transform.rotation);
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
            DirectionY = _direction.y
        };
        return staticTileInfo;
    }

    public override bool Deserialize(ISerializableTileInfo tileInfo)
    {
        throw new System.NotImplementedException();
    }
}
