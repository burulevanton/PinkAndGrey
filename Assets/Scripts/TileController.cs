using Serialize;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class TileController : MonoBehaviour, ISerializableMonoBehaviour
    {
        public abstract ISerializableTileInfo Serialize();

        public abstract bool Deserialize(ISerializableTileInfo tileInfo);
    }
}