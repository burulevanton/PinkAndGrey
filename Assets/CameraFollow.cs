using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    
    [SerializeField]
    protected float followSpeed;
    

    void LateUpdate ()
    {
        float xNew = Mathf.Lerp(transform.position.x, target.position.x, Time.deltaTime * followSpeed);
        float yNew = Mathf.Lerp(transform.position.y, target.position.y, Time.deltaTime * followSpeed);
        transform.position = new Vector3(xNew, yNew, transform.position.z);
    }
}
