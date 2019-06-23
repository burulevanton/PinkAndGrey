using System;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    public GameObject BulletExplodePrefab;

    public Vector3 Direction; 

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.IsPaused)
            return;
        var position = transform.position;
        var vector = _speed * Time.deltaTime * Direction;
        float distance = Mathf.Abs((double) vector.x == 0.0 ? vector.y : vector.x);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(position + Direction * 0.5f, this.Direction, distance);
        bool stop = false;
        if (raycastHit2D.collider != null)
        {
            switch (raycastHit2D.collider.tag)
            {
                case "TimerWall":
                    var clone = raycastHit2D.collider.gameObject.GetComponent<TimerWallController>();
                    if(clone.IsActivated)
                        stop = StopByTransform(raycastHit2D.point);
                    break;
                case "Wall":
                    stop = StopByTransform(raycastHit2D.point);
                    break;
                case "Spike":
                    stop = StopByTransform(raycastHit2D.point);
                    break;
                case "LaserTrap":
                    stop = StopByTransform(raycastHit2D.point);
                    break;
//                case "Player":
//                    PoolManager.ReleaseObject(gameObject);
//                    break;
            }
//            if (raycastHit2D.collider.tag == "TimerWall")
//            {
//                var clone = raycastHit2D.collider.gameObject.GetComponent<TimerWallController>();
//                if(clone.IsActivated)
//                    stop = StopByTransform(raycastHit2D.point);
//            }
//            else
//            {
//                stop = StopByTransform(raycastHit2D.point);
//            }
        }
        if(!stop)
            transform.position += vector;
    }

    private bool StopByTransform(Vector2 vector)
    {
        vector -= (Vector2)Direction * 0.5f;
        var rotation = Direction.x == 0
                        ? Direction.y == 1 ? new Vector3(0, 0, 180f) : new Vector3(0, 0, 0)
                        : new Vector3(0, 0, Direction.x * 90f);
        PoolManager.ReleaseObject(gameObject);
        PoolManager.SpawnObject(BulletExplodePrefab, vector, Quaternion.Euler(rotation));
        return true;
    }
    
}
