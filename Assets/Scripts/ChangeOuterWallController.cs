using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOuterWallController : MonoBehaviour
{
    [SerializeField] private OuterWallsController _outerWall;

    private void OnTriggerEnter2D(Collider2D other)
    {
        _outerWall.ChangeDirection();
    }
}
