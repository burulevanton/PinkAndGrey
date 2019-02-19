using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallController: MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _outerWalls;
    [SerializeField] private GameObject _nextLocation;
    private Vector3 _prevLocation = new Vector3();

    private void OnTriggerExit2D(Collider2D other)
    {
        _prevLocation = _outerWalls.transform.position;
        _camera.transform.position = new Vector3(_nextLocation.transform.position.x, _nextLocation.transform.position.y, _camera.transform.position.z);
        _outerWalls.transform.position = _nextLocation.transform.position;
        _nextLocation.transform.position = _prevLocation;
    }
}
