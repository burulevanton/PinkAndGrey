using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    public Vector3 Direction; 

    // Update is called once per frame
    void Update()
    {
        transform.position += Direction * Time.deltaTime * _speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TimerWall"))
        {
            var clone = other.gameObject.GetComponent<TimerWallController>();
            if (!clone.IsActivated)
                return;
        }
        PoolManager.ReleaseObject(gameObject);
    }
}
