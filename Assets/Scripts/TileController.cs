using Serialize;
using UnityEngine;

public abstract class TileController : MonoBehaviour, ISerializableMonoBehaviour
{
    public abstract StaticTileInfo Serialize();

    public abstract bool Deserialize(StaticTileInfo tileInfo);

//    private void Update()
//    {
//        if (GameController.Instance.IsPaused)
//        {
//            return;
//        }
//        OnUpdate();
//    }
//
//    protected abstract void OnUpdate();
//
//    private void FixedUpdate()
//    {
//        if (GameController.Instance.IsPaused)
//        {
//            return;
//        }
//        OnFixedUpdate();
//    }
//
//    protected abstract void OnFixedUpdate();
}