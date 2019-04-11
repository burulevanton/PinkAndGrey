namespace Serialize
{
    public interface ISerializableMonoBehaviour
    {
        StaticTileInfo Serialize();
        bool Deserialize(StaticTileInfo tileInfo);
    }
}