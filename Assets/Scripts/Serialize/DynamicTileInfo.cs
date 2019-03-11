using Enum;

namespace Serialize
{
    public class DynamicTileInfo:ISerializableTileInfo
    {
        public TileType TileType { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float FromDirectionX { get; set; }
        public float FromDirectionY { get; set; }
        public float ToDirectionX { get; set; }
        public float ToDirectionY { get; set; }
    }
}