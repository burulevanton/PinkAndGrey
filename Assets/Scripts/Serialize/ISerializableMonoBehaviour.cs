namespace Serialize
{
    public interface ISerializableMonoBehaviour
    {
        ISerializableTileInfo Serialize();
        bool Deserialize(ISerializableTileInfo tileInfo);
    }
}