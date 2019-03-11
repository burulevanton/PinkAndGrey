using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using DefaultNamespace;
using Newtonsoft.Json;
using Serialize;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    public void Serialize()
    {
        var breakingPlatformControllers =
            Object.FindObjectsOfType<TileController>();
        var infos = breakingPlatformControllers.Select(breakingPlatformController => breakingPlatformController.Serialize()).ToList();
        var json = JsonConvert.SerializeObject(infos, Formatting.Indented);
        Debug.Log(json);
    }
}