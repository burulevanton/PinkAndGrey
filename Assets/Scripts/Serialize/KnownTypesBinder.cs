using System;
using System.Collections.Generic;
using System.Linq;
using Enum;
using Newtonsoft.Json.Serialization;

namespace Serialize
{
    public class KnownTypesBinder : DefaultSerializationBinder
    {

        public override Type BindToType(string assemblyName, string typeName)
        {
            switch (typeName)
            {
                case "Serialize.PortalTileInfo":
                    return typeof(PortalTileInfo);
                case "Serialize.DynamicTileInfo":
                    return typeof(DynamicTileInfo);
                case "Serialize.StaticTileWithSomeDirectionInfo":
                    return typeof(StaticTileWithSomeDirectionInfo);
                case "Serialize.StaticTileInfo":
                    return typeof(StaticTileInfo);
                default:
                    return base.BindToType(assemblyName, typeName);
            }
        }
    }
}