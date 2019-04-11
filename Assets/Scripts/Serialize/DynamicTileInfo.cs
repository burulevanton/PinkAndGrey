using Enum;

namespace Serialize
{
    public class DynamicTileInfo:StaticTileInfo
    {
        public float FromDirectionX { get; set; }
        public float FromDirectionY { get; set; }
        public float ToDirectionX { get; set; }
        public float ToDirectionY { get; set; }
    }
}