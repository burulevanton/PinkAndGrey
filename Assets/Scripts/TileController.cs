using Serialize;
using UnityEngine;

public abstract class TileController : MonoBehaviour, ISerializableMonoBehaviour
{
    public abstract StaticTileInfo Serialize();

    public abstract bool Deserialize(StaticTileInfo tileInfo);
}