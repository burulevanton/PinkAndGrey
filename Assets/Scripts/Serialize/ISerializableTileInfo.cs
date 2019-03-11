using Enum;

namespace Serialize
{
    public interface ISerializableTileInfo
    {
        TileType TileType { get; set; }
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }
    }
}