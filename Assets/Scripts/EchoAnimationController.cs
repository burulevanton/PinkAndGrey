using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;

public class EchoAnimationController : MonoBehaviour
{
    void OnAnimationEnd()
    {
        PoolManager.ReleaseObject(this.gameObject);
    }
}
