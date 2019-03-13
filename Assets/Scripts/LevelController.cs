using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using DefaultNamespace;
using Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectPool;
using Serialize;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{

    [SerializeField] private GameObject BreakingPlatformPrefab;
    [SerializeField] private GameObject CannonPrefab;
    [SerializeField] private GameObject CollectablePrefab;
    [SerializeField] private GameObject CopyingPortalPrefab;
    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private GameObject GreaterSpikePrefab;
    [SerializeField] private GameObject MovingChangingPlatformPrefab;
    [SerializeField] private GameObject MovingPlatformPrefab;
    [SerializeField] private GameObject PortalPrefab;
    [SerializeField] private GameObject SpikePrefab;
    [SerializeField] private GameObject TimerWallPrefab;
    [SerializeField] private GameObject InnerWallPrefab;
    
    public void Serialize()
    {
        var tileControllers =
            Object.FindObjectsOfType<TileController>();
        var infos = tileControllers.Select(x => x.Serialize()).ToList();
        var json = JsonConvert.SerializeObject(infos, Formatting.Indented);
        string path = Application.dataPath + "/Levels/1.json";
        Debug.Log(path);
        File.WriteAllText(path, json, Encoding.UTF8);
        Debug.Log(json);
    }

    public void Deserialize()
    {
        string path = Application.dataPath + "/Levels/1.json";
        var json = File.ReadAllText(path);
        var tilesArray = JArray.Parse(json);
        foreach (var tile in tilesArray)
        {
            var type = (TileType) (int) tile["TileType"];
            switch (type)
            {
                    case TileType.BreakingPlatform:
                        var breakingPlatformClone = PoolManager.SpawnObject(BreakingPlatformPrefab)
                            .GetComponent<BreakingPlatformController>();
                        breakingPlatformClone
                            .Deserialize(JsonConvert.DeserializeObject<StaticTileInfo>(tile.ToString()));
                        break;
                    case TileType.InnerWall:
                        var innerWallClone =
                            PoolManager.SpawnObject(InnerWallPrefab).GetComponent<InnerWallController>();
                        innerWallClone.Deserialize(JsonConvert.DeserializeObject<StaticTileInfo>(tile.ToString()));
                        break;
                    case TileType.MovingChangingPlatform:
                        var movingChangingPlatformClone = PoolManager.SpawnObject(MovingChangingPlatformPrefab)
                            .GetComponent<MovingChangingPlatform>();
                        movingChangingPlatformClone.Deserialize(
                            JsonConvert.DeserializeObject<StaticTileWithSomeDirectionInfo>(tile.ToString()));
                        break;
                    case TileType.CopyingPortal:
                        var copyingPortalClone = PoolManager.SpawnObject(CopyingPortalPrefab)
                            .GetComponent<CopyingPortalController>();
                        copyingPortalClone.Deserialize(JsonConvert.DeserializeObject<StaticTileInfo>(tile.ToString()));
                        break;
                    case TileType.Cannon:
                        var cannonClone = PoolManager.SpawnObject(CannonPrefab).GetComponent<CannonController>();
                        cannonClone.Deserialize(
                            JsonConvert.DeserializeObject<StaticTileWithSomeDirectionInfo>(tile.ToString()));
                        break;
                    case TileType.Collectable:
                        var collectableClone = PoolManager.SpawnObject(CollectablePrefab).GetComponent<Collectable>();
                        collectableClone.Deserialize(JsonConvert.DeserializeObject<StaticTileInfo>(tile.ToString()));
                        break;
                    case TileType.Enemy:
                        
                        break;
            }
        }
    }
}