using Enum;

namespace Serialize
{
    public class StaticTileWithSomeDirectionInfo: ISerializableTileInfo
    {
        public float DirectionX { get; set; }
        public float DirectionY { get; set; }
        public TileType TileType { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float RotationZ { get; set; }
    }
}