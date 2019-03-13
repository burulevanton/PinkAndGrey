using Enum;

namespace Serialize
{
    public class PortalTileInfo: ISerializableTileInfo
    {
        public TileType TileType { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float OtherPortalX { get; set; }
        public float OtherPortalY { get; set; }
        public float OtherPortalZ { get; set; }
    }
}