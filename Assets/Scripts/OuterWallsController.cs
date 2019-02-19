using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


public class OuterWallsController : MonoBehaviour
{
    [FormerlySerializedAs("wallPrefab")] [SerializeField] private GameObject _wallPrefab;

    enum Direction
    {
        Left,
        Top,
        Down,
        Right
    };

    private Direction _direction = Direction.Down;
    
    private float _minX = -7.5f;
    private float _maxX = 7.5f;
    private float _minY = -6.5f;
    private float _maxY = 6.5f;

    private GameObject _top;
    private GameObject _down;
    private GameObject _left;
    private GameObject _right;
    
    void Start()
    {
        _top = new GameObject("Top");
        _top.transform.parent = transform;
        _down = new GameObject("Down");
        _down.transform.parent = transform;
        _left = new GameObject("Left");
        _left.transform.parent = transform;
        _right = new GameObject("Right");
        _right.transform.parent = transform;
        _minX += transform.position.x;
        _maxX += transform.position.x;
        _minY += transform.position.y;
        _maxY += transform.position.y;
        //top and down
        for (var i = 0; i < 14; i++)
        {
            var tempTop = Instantiate(_wallPrefab, new Vector3(_minX + 1 + i, _maxY, 0), Quaternion.identity, _top.transform); //top
            var tempDown = Instantiate(_wallPrefab, new Vector3(_minX + 1 + i, _minY, 0), Quaternion.identity, _down.transform); //down
        }
        //left and right
        for (var i = 0; i < 12; i++)
        {
            var tempLeft = Instantiate(_wallPrefab, new Vector3(_minX, _maxY-1-i, 0), Quaternion.identity, _left.transform); //left
            var tempRight = Instantiate(_wallPrefab, new Vector3(_maxX, _maxY-1-i, 0), Quaternion.identity, _right.transform); //right
        }
        var leftTopCorner = Instantiate(_wallPrefab, new Vector3(_minX, _maxY, 0), Quaternion.identity, transform);
        var leftDownCorner = Instantiate(_wallPrefab, new Vector3(_minX, _minY, 0), Quaternion.identity, transform);
        var rightTopCorner = Instantiate(_wallPrefab, new Vector3(_maxX, _maxY, 0), Quaternion.identity, transform);
        var rightDownCorner = Instantiate(_wallPrefab, new Vector3(_maxX, _minY, 0), Quaternion.identity, transform);
        RemoveDown();
        //transform.position = new Vector3(transform.position.x, 7.0f, transform.position.z);
        //StartCoroutine(Test());
    }

    public void RemoveLeft()
    {
        _down.SetActive(true);
        _left.SetActive(false);
    }

    public void RemoveTop()
    {
        _left.SetActive(true);
        _top.SetActive(false);
    }

    public void RemoveRight()
    {
        _top.SetActive(true);
        _right.SetActive(false);
    }

    public void RemoveDown()
    {
        _right.SetActive(true);
        _down.SetActive(false);
    }

    IEnumerator Test()
    {
        while (true)
        {
            ChangeDirection();
            yield return new WaitForSeconds(2f);
        }
    }

    public void ChangeDirection()
    {
        Debug.Log(_direction);
        switch (_direction)
        {
                case Direction.Down:
                    RemoveLeft();
                    _direction = Direction.Left;
                    return;
                case Direction.Left:
                    RemoveTop();
                    _direction = Direction.Top;
                    return;
                case Direction.Top:
                    RemoveRight();
                    _direction = Direction.Right;
                    return;
                case Direction.Right:
                    RemoveDown();
                    _direction = Direction.Down;
                    return;
        }
    }

}
