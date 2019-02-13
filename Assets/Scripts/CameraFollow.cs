using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    
    [SerializeField]
    protected float followSpeed;
    
    private float _pixelLockedPPU = 16.0f;
    

    void LateUpdate ()
    {
        float xNew = Mathf.Lerp(transform.position.x, target.position.x, Time.deltaTime * followSpeed);
        float yNew = Mathf.Lerp(transform.position.y, target.position.y, Time.deltaTime * followSpeed);
        transform.position = new Vector3(xNew, yNew, transform.position.z);
        //Vector2 newPosition = new Vector2(target.transform.position.x, target.transform.position.y);
        //float nextX = Mathf.Round(_pixelLockedPPU * newPosition.x);
        //float nextY = Mathf.Round(_pixelLockedPPU * newPosition.y);
        //transform.position = new Vector3(nextX/_pixelLockedPPU, nextY/_pixelLockedPPU, transform.position.z);
    }
}
