using Enum;

namespace Serialize
{
    public class StaticTileInfo: ISerializableTileInfo
    {
        public TileType TileType { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}