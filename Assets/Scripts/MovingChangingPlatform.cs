using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingChangingPlatform : MonoBehaviour
{
    [SerializeField]
    private Vector2 _direction = Vector2.one;

    public Vector2 Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }
}
