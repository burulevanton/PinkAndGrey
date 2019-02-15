using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 _fromDirection;
    [SerializeField] private Vector3 _toDirection;
    [SerializeField] private float moveSpeed;

    [SerializeField] private bool reverseMove = false;
    private float _startTime;
    private float _journeyLength;
    private float _distCovered;
    private float _fracJourney;
    private bool _isPause = false;
    void Start()
    {
        _startTime = Time.time;
        _journeyLength = Vector3.Distance(_fromDirection, _toDirection);
    }
    void Update()
    {
                
        if (_isPause)
        {
            if (Time.time - _startTime > 0.5f)
            {
                _isPause = false;
                _startTime = Time.time;
            }
            return;
        }
        _distCovered = (Time.time - _startTime) * moveSpeed;
        _fracJourney = _distCovered / _journeyLength;
        if (reverseMove)
        {
            transform.position = Vector3.Lerp(_fromDirection, _toDirection, _fracJourney);
        }
        else
        {
            transform.position = Vector3.Lerp(_toDirection, _fromDirection, _fracJourney);
        }
        if ((Vector3.Distance(transform.position, _toDirection) == 0.0f || Vector3.Distance(transform.position, _fromDirection) == 0.0f))
        {

            reverseMove = !reverseMove;
            _isPause = true;
            _startTime = Time.time;
        }
    }
}
