using ObjectPool;
using UnityEngine;

namespace DefaultNamespace
{
    public class BulletExplodeController : MonoBehaviour
    {
        public void OnAnimationEnd()
        {
            PoolManager.ReleaseObject(gameObject);
        }
    }
}