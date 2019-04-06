using System;
using System.Collections;
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
using Application = UnityEngine.Application;
using Object = UnityEngine.Object;

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
    [SerializeField] private GameObject LevelEndPrefab;
   
    
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

    public IEnumerator Deserialize()
    {
        yield return StartCoroutine(ClearScene());
        GameController.Instance.Text.text = "Очистка уровня завершена";
        //string path = Application.dataPath + "/Levels/" + GameData.Instance.CurrentLevel +".json";
//        string path = Application.persistentDataPath + "/Levels/1.json";
//        GameController.Instance.Text.text = "Получение файла";
//        GameController.Instance.Text.text = path;
//        string json;
//        try
//        {
//            json = File.ReadAllText(path);
//        }
//        catch (Exception e)
//        {
//            GameController.Instance.Text.text = e.ToString();
//            throw;
//        }
//        GameController.Instance.Text.text = "Jnrhsnbt файла";
        var path = "Levels/" + GameData.Instance.CurrentLevel;
        var json = Resources.Load<TextAsset>(path).text;
        var tilesArray = JArray.Parse(json);
        List<GameObject> portalsList = new List<GameObject>();
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
                    case TileType.LevelEnd:
                        var levelEndClone = PoolManager.SpawnObject(LevelEndPrefab).GetComponent<LevelEndController>();
                        levelEndClone.Deserialize(JsonConvert.DeserializeObject<StaticTileInfo>(tile.ToString()));
                        break;
            }
            yield return null;
        }
    }

    public IEnumerator ClearScene()
    {
        var tileControllers =
            Object.FindObjectsOfType<TileController>();
        foreach (var tileController in tileControllers)
        {
            PoolManager.ReleaseObject(tileController.gameObject);
            yield return null;
        }
    }

    private void CheckForOtherPortal(List<GameObject> portals, Vector3 portalPosition, PortalController portalController)
    {
        foreach (var portal in portals)
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