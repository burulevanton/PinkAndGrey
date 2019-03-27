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

    private List<GameObject> portalsList = new List<GameObject>();
   
    
    public void Serialize()
    {
        var tileControllers =
            Object.FindObjectsOfType<TileController>();
        var infos = tileControllers.Select(x => x.Serialize()).ToList();
        var json = JsonConvert.SerializeObject(infos, Formatting.Indented);
        string path = Application.dataPath + "/Levels/2.json";
        Debug.Log(path);
        File.WriteAllText(path, json, Encoding.UTF8);
        Debug.Log(json);
    }

    public void Deserialize()
    {
        string path = Application.dataPath + "/Levels/2.json";
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
                        var enemyClone = PoolManager.SpawnObject(EnemyPrefab).GetComponent<EnemyController>();
                        enemyClone.Deserialize(JsonConvert.DeserializeObject<DynamicTileInfo>(tile.ToString()));
                        break;
                    case TileType.GreaterSpike:
                        var greaterSpikeClone = PoolManager.SpawnObject(GreaterSpikePrefab)
                            .GetComponent<GreaterSpike>();
                        greaterSpikeClone.Deserialize(
                            JsonConvert.DeserializeObject<StaticTileWithSomeDirectionInfo>(tile.ToString()));
                        break;
                    case TileType.MovingPlatform:
                        var movingPlatformClone = PoolManager.SpawnObject(MovingPlatformPrefab).GetComponent<MovingPlatform>();
                        movingPlatformClone.Deserialize(
                            JsonConvert.DeserializeObject<DynamicTileInfo>(tile.ToString()));
                        break;
                    case TileType.Spike:
                        var spikeClone = PoolManager.SpawnObject(SpikePrefab).GetComponent<SpikeController>();
                        spikeClone.Deserialize(JsonConvert.DeserializeObject<StaticTileInfo>(tile.ToString()));
                        break;
                    case TileType.TimerWall:
                        var timerWallClone =
                            PoolManager.SpawnObject(TimerWallPrefab).GetComponent<TimerWallController>();
                        timerWallClone.Deserialize(JsonConvert.DeserializeObject<StaticTileInfo>(tile.ToString()));
                        break;
                    case TileType.Portal:
                        var portalClone = PoolManager.SpawnObject(PortalPrefab).GetComponent<PortalController>();
                        var portalTileInfo = JsonConvert.DeserializeObject<PortalTileInfo>(tile.ToString());
                        portalClone.Deserialize(portalTileInfo);
                        portalsList.Add(portalClone.gameObject);
                        CheckForOtherPortal(portalsList, new Vector3(portalTileInfo.OtherPortalX,
                            portalTileInfo.OtherPortalY,
                            portalTileInfo.OtherPortalZ), portalClone);
                        break;
            }
        }
    }

    private void CheckForOtherPortal(List<GameObject> portals, Vector3 portalPosition, PortalController portalController)
    {
        foreach (var portal in portalsList)
        {
            if (portal.transform.position == portalPosition)
            {
                portals.Remove(portal);
                portalController.OtherPortal = portal;
                portal.GetComponent<PortalController>().OtherPortal = portalController.gameObject;
                portals.Remove(portalController.gameObject);
                return;
            }
        }
    }
}