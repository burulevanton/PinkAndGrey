using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectPool;
using Serialize;
using UnityEngine;
using UnityEngine.Tilemaps;
using Application = UnityEngine.Application;
using Object = UnityEngine.Object;

public class LevelController : Singleton<LevelController>
{

    [SerializeField] private GameObject BreakingPlatformPrefab;
    [SerializeField] private GameObject CannonHorizontalPrefab;
    [SerializeField] private GameObject CannonVerticalPrefab;
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

    [SerializeField] private GameObject _breakingPlatforms;
    [SerializeField] private GameObject _cannons;
    [SerializeField] private GameObject _collectables;
    [SerializeField] private GameObject _copyingPortals;
    [SerializeField] private GameObject _enemies;
    [SerializeField] private GameObject _greaterSpikes;
    [SerializeField] private GameObject _innerWalls;
    [SerializeField] private GameObject _levelEnds;
    [SerializeField] private GameObject _movingChangingPlatforms;
    [SerializeField] private GameObject _movingPlatforms;
    [SerializeField] private GameObject _portals;
    [SerializeField] private GameObject _spikes;
    [SerializeField] private GameObject _timerWalls;

    [SerializeField] private Tilemap tilemapEnvironment;
    [SerializeField] private List<TileBase> tiles;
    
    public void Serialize()
    {
        var tileControllers =
            Object.FindObjectsOfType<TileController>();
        var infos = tileControllers.Select(x => x.Serialize()).ToList();
        var dict = new Dictionary<TileType, List<StaticTileInfo>>();
        foreach (var info in infos)
        {
            if (dict.ContainsKey(info.TileType))
            {
                dict[info.TileType].Add(info);
            }
            else
            {
                dict.Add(info.TileType, new List<StaticTileInfo>() {info});
            }
        }
        
        var levelInfo = new LevelInfo
        {
        TileGameObjects = dict, PlayerPos = GameController.Instance.PlayerController.transform.position,
        PaletteTileInfos = SerializeEnvironment()
        };
        var json = JsonConvert.SerializeObject(levelInfo, Formatting.Indented, new JsonSerializerSettings()
        {
        TypeNameHandling = TypeNameHandling.Auto,
        Binder = new KnownTypesBinder()
        });
        string path = Application.dataPath + "/Levels/3.json";
        Debug.Log(path);
        File.WriteAllText(path, json, Encoding.UTF8);
        Debug.Log(json);
    }

    private List<PaletteTileInfo> SerializeEnvironment()
    {
        var list = new List<PaletteTileInfo>();
        for (int i = tilemapEnvironment.cellBounds.xMin; i < tilemapEnvironment.cellBounds.xMax; i++)
        {
            for (int j = tilemapEnvironment.cellBounds.yMin; j < tilemapEnvironment.cellBounds.yMax; j++)
            {
                Vector3Int localPlace = (new Vector3Int(i, j, (int) tilemapEnvironment.transform.position.y));
                if (tilemapEnvironment.HasTile(localPlace))
                {
                    var tile = tilemapEnvironment.GetTile(localPlace);
                    var tileInfo = new PaletteTileInfo
                    {
                    X = i, Y = j, IndexOfArray = tiles.FindIndex(x => x.name == tile.name)
                    };
                    list.Add(tileInfo);
                }
            }
        }

        return list;
    }

    public IEnumerator Deserialize()
    {
        yield return StartCoroutine(PoolManager.Instance.ClearScene());
//        var path = "Levels/" + GameData.Instance.CurrentLevel;
        var path = "Levels/3";
        var json = Resources.Load<TextAsset>(path).text;
        JObject jObject = JObject.Parse(json);
        GameController.Instance.PlayerController.transform.position =
        JsonConvert.DeserializeObject<Vector3>(jObject["PlayerPos"].ToString());
        var tilesDict =
        JsonConvert.DeserializeObject<Dictionary<TileType, List<StaticTileInfo>>>(
        jObject["TileGameObjects"].ToString(), new JsonSerializerSettings()
        {
        TypeNameHandling = TypeNameHandling.Objects,
        Binder = new KnownTypesBinder()
        });
        foreach (var keypair in tilesDict)
        {
            DeserializeList(keypair.Key, keypair.Value);
            yield return null;
        }
        var environmentList =
        JsonConvert.DeserializeObject<List<PaletteTileInfo>>(jObject["PaletteTileInfos"].ToString());
        DeserializeEnvironment(environmentList);
        yield return null;
    }

    private void DeserializeEnvironment(List<PaletteTileInfo> list)
    {
        foreach (var l in list)
        {
            tilemapEnvironment.SetTile(new Vector3Int(l.X,l.Y, 0), tiles[l.IndexOfArray]);
        }
    }

    private void DeserializeList(TileType type, List<StaticTileInfo> tilesArray)
    {
        List<GameObject> portalsList = new List<GameObject>();
        foreach (var tile in tilesArray)
        {
            switch (type)
            {
                    case TileType.BreakingPlatform:
                        var breakingPlatformClone = PoolManager.SpawnObject(BreakingPlatformPrefab)
                            .GetComponent<BreakingPlatformController>();
                        breakingPlatformClone
                            .Deserialize(tile);
                        breakingPlatformClone.transform.parent = _breakingPlatforms.transform;
                        break;
                    case TileType.MovingChangingPlatform:
                        var movingChangingPlatformClone = PoolManager.SpawnObject(MovingChangingPlatformPrefab)
                            .GetComponent<MovingChangingPlatform>();
                        movingChangingPlatformClone.Deserialize(tile);
                        movingChangingPlatformClone.transform.parent = _movingChangingPlatforms.transform;
                        break;
                    case TileType.CopyingPortal:
                        var copyingPortalClone = PoolManager.SpawnObject(CopyingPortalPrefab)
                            .GetComponent<CopyingPortalController>();
                        copyingPortalClone.Deserialize(tile);
                        copyingPortalClone.transform.parent = _copyingPortals.transform;
                        break;
                    case TileType.CannonHorizontal:
                        var cannonHorizontalClone = PoolManager.SpawnObject(CannonHorizontalPrefab).GetComponent<CannonController>();
                        cannonHorizontalClone.Deserialize(tile);
                        cannonHorizontalClone.transform.parent = _cannons.transform;
                        break;
                    case TileType.CannonVertical:
                        var cannonVerticalClone = PoolManager.SpawnObject(CannonVerticalPrefab).GetComponent<CannonController>();
                        cannonVerticalClone.Deserialize(tile);
                        cannonVerticalClone.transform.parent = _cannons.transform;
                        break;
                    case TileType.Collectable:
                        var collectableClone = PoolManager.SpawnObject(CollectablePrefab).GetComponent<Collectable>();
                        collectableClone.Deserialize(tile);
                        collectableClone.transform.parent = _collectables.transform;
                        break;
                    case TileType.Enemy:
                        var enemyClone = PoolManager.SpawnObject(EnemyPrefab).GetComponent<EnemyController>();
                        enemyClone.Deserialize(tile);
                        enemyClone.transform.parent = _enemies.transform;
                        break;
                    case TileType.GreaterSpike:
                        var greaterSpikeClone = PoolManager.SpawnObject(GreaterSpikePrefab)
                            .GetComponent<LaserTrapController>();
                        greaterSpikeClone.Deserialize(tile);
                        greaterSpikeClone.transform.parent = _greaterSpikes.transform;
                        break;
                    case TileType.MovingPlatform:
                        var movingPlatformClone = PoolManager.SpawnObject(MovingPlatformPrefab).GetComponent<MovingPlatform>();
                        movingPlatformClone.Deserialize(tile);
                        movingPlatformClone.transform.parent = _movingPlatforms.transform;
                        break;
                    case TileType.Spike:
                        var spikeClone = PoolManager.SpawnObject(SpikePrefab).GetComponent<SpikeController>();
                        spikeClone.Deserialize(tile);
                        spikeClone.transform.parent = _spikes.transform;
                        break;
                    case TileType.TimerWall:
                        var timerWallClone =
                            PoolManager.SpawnObject(TimerWallPrefab).GetComponent<TimerWallController>();
                        timerWallClone.Deserialize(tile);
                        timerWallClone.transform.parent = _timerWalls.transform;
                        break;
                    case TileType.Portal:
                        var portalClone = PoolManager.SpawnObject(PortalPrefab).GetComponent<PortalController>();
                        portalClone.Deserialize(tile);
                        portalsList.Add(portalClone.gameObject);
                        var portalTileInfo = tile as PortalTileInfo;
                        CheckForOtherPortal(portalsList, portalTileInfo.OtherPortalPosition, portalClone);
                        portalClone.transform.parent = _portals.transform;
                        break;
                    case TileType.LevelEnd:
                        var levelEndClone = PoolManager.SpawnObject(LevelEndPrefab).GetComponent<LevelEndController>();
                        levelEndClone.Deserialize(tile);
                        levelEndClone.transform.parent = _levelEnds.transform;
                        break;
                    case TileType.InnerWall:
                        var InnerWallClone =
                        PoolManager.SpawnObject(InnerWallPrefab).GetComponent<InnerWallController>();
                        InnerWallClone.Deserialize(tile);
                        InnerWallClone.transform.parent = _innerWalls.transform;
                        break;
            }
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