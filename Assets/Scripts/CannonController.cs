using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class CannonController : MonoBehaviour
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
}
