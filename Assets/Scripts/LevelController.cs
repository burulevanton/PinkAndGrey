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
    [SerializeField] private GameObject PlayerPrefab;

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
    [SerializeField] private GameObject _playerWrap;

    [SerializeField] private Tilemap tilemapEnvironment;
    [SerializeField] private Tilemap tilemapWalls;
    [SerializeField] private Tilemap tilemapSpikes;
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
        PaletteTileInfos = SerializeTileMap(tilemapEnvironment),
        TileWalls = SerializeTileMap(tilemapWalls),
        TileSpikes = SerializeTileMap(tilemapSpikes)
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

    private List<PaletteTileInfo> SerializeTileMap(Tilemap tilemap)
    {
        var list = new List<PaletteTileInfo>();
        for (int i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
        {
            for (int j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
            {
                Vector3Int localPlace = (new Vector3Int(i, j, (int) tilemap.transform.position.y));
                if (tilemap.HasTile(localPlace))
                {
                    var tile = tilemap.GetTile(localPlace);
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
        ClearScene();
        var path = "Levels/" + GameData.Instance.CurrentLevel;
        //var path = "Levels/3";
        var json = Resources.Load<TextAsset>(path).text;
        JObject jObject = JObject.Parse(json);
//        GameController.Instance.PlayerController = PoolManager.SpawnObject(PlayerPrefab,
//        JsonConvert.DeserializeObject<Vector3>(jObject["PlayerPos"].ToString()), Quaternion.identity)
//        .GetComponent<PlayerController>();
//        GameController.Instance.PlayerController.transform.parent = _playerWrap.transform;
        GameController.Instance.PlayerController.transform.parent = null;
        GameController.Instance.PlayerController.transform.position =
        JsonConvert.DeserializeObject<Vector3>(jObject["PlayerPos"].ToString());
        GameController.Instance.PlayerController.transform.rotation = Quaternion.identity;
        GameController.Instance.PlayerController.IsAlive();
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
//            yield return null;
        }
        var environmentList =
        JsonConvert.DeserializeObject<List<PaletteTileInfo>>(jObject["PaletteTileInfos"].ToString());
        DeserializeTilemap(environmentList, tilemapEnvironment);
        var wallList = JsonConvert.DeserializeObject<List<PaletteTileInfo>>(jObject["TileWalls"].ToString());
        DeserializeTilemap(wallList, tilemapWalls);
        if (jObject["TileSpikes"] != null)
        {
            var spikesList = JsonConvert.DeserializeObject<List<PaletteTileInfo>>(jObject["TileSpikes"].ToString());
            DeserializeTilemap(spikesList, tilemapSpikes);
        }
        yield return null;
    }

    private void ClearScene()
    {
        tilemapEnvironment.ClearAllTiles();
        tilemapWalls.ClearAllTiles();
        tilemapSpikes.ClearAllTiles();
        var tileControllers = GetComponentsInChildren<TileController>(false);
        foreach (var tileController in tileControllers)
        {
            PoolManager.ReleaseObject(tileController.gameObject);
        }
//        if (GameController.Instance.PlayerController != null)
//        {
//            PoolManager.ReleaseObject(GameController.Instance.PlayerController.gameObject);            
//        }
    }

    private void DeserializeTilemap(List<PaletteTileInfo> list, Tilemap tilemap)
    {
        foreach (var l in list)
        {
            tilemap.SetTile(new Vector3Int(l.X,l.Y, 0), tiles[l.IndexOfArray]);
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
                    case TileType.LaserTrap:
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