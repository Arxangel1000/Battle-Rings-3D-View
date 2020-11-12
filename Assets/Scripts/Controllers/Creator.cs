using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Creator : MonoBehaviour
{
    public static Creator instance;
    public List<Vector3> correctPos = new List<Vector3>();
    private List<GameObject> dummys = new List<GameObject>();
    public List<float> teamRadius = new List<float>();

    public static List<GameObject> poolDummys = new List<GameObject>();
    public static List<GameObject> poolUnits = new List<GameObject>();
    
    private static bool notFirstBornOnGame;
    public bool canOverlaping;
    
    public enum RingColor { blue, red }

    public RingColor color = RingColor.blue;
    
    private void Awake()
    {
        instance = this;
        StartCoroutine(PreparationForGames());
    }

    IEnumerator PreparationForGames()
    {
        CreateBattleField();
        
        if (!notFirstBornOnGame)
        {
            PoolCreator(BuildDummy(), poolDummys);
            PoolCreator(BuildDummy(), poolDummys);
            yield return new WaitForSeconds(1f);
            PoolCreator(BuildRing(RingColor.blue), poolUnits);
            PoolCreator(BuildRing(RingColor.red), poolUnits);

            notFirstBornOnGame = true;
        }
        else
        {
            RestartCreator();
        }
        
        FindCorrectPos(poolDummys);
        yield return new WaitForSeconds(1f);
        GetCorrectPositions();
        HideDummys();
        TeamsRadiusCollection();
        yield return new WaitForSeconds(1f);
        StartCoroutine(CreateAllUnits());
    }

    public void RestartCreator()
    {
        canOverlaping = false;
        ClearSceneTheUnits();
        VisibleDummys();
    }

    private void GetCorrectPositions()
    {
        foreach (var dommuPos in poolDummys)
        {
            correctPos.Add(dommuPos.transform.position);
        }
    }

    private void PoolCreator(GameObject go, List<GameObject> list)
    {
        GameData.currentCountUnit = 0;
            while (GameData.currentCountUnit != GameData.numUnitsToSpawn)
            {
                GameObject unit = Instantiate(go);
                list.Add(unit);
                GameData.currentCountUnit++;
            }
    }
    
    private void TeamsRadiusCollection()
    {
        for (int i = 0; i < GameData.numUnitsToSpawn; i++)
        {
            teamRadius.Add(Random.Range(GameData.unitSpawnMinRadius, GameData.unitSpawnMaxRadius));
        }

        for (int i = 0; i < GameData.numUnitsToSpawn; i++)
        {
            teamRadius.Add(teamRadius[i]);
        }
    }

    private void CreateBattleField()
    {
        GameObject prefab =
            Instantiate(BuildBattleField(GameData.gameAreaWidth, GameData.gameAreaHeight));
    }
    
    private void ClearSceneTheUnits()
    {
        foreach (var unit in poolUnits)
        {
            unit.SetActive(false);
        }
    }

    private void FindCorrectPos(List<GameObject> list)
    {
        canOverlaping = true;
        for (int i = 0; i < list.Count; i++)
        {
            poolDummys[i].transform.localScale = new Vector2(GameData.unitSpawnMaxRadius, GameData.unitSpawnMaxRadius);
            poolDummys[i].transform.position = SpawnPoint();
        }
    }
    
    private void CreateUnitFromPool(List<GameObject> units)
    { 
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            for (int i = 0; i < GameData.numUnitsToSpawn * 2; i++)
            {
                GameData.currentCountUnit++;
                yield return new WaitForSeconds(GameData.unitSpawnDelay);
                units[i].SetActive(true);
                units[i].GetComponent<RingModel>().unitSpawnRadius = teamRadius[i];
                units[i].GetComponent<RingModel>().unitCurrentRadius =
                    units[i].GetComponent<RingModel>().unitSpawnRadius;
                units[i].GetComponent<RingView>().LoadScaleUnit();
                units[i].GetComponent<RingController>().AddTeamSumRadius();
                units[i].transform.position = correctPos[i];
                units[i].GetComponent<SpriteRenderer>().enabled = true;
            }
            EventSpawnIsFinished.OnSpawnIsFinished();
        }
    }
    
    IEnumerator CreateAllUnits()
    {
        GameData.currentCountUnit = 0;

            yield return new WaitForSeconds(GameData.unitSpawnDelay);
            CreateUnitFromPool(poolUnits);
    }

    public Vector3 SpawnPoint()
    {
        float minX = GameData.gameAreaWidth * (-2.1f);
        float maxX = GameData.gameAreaHeight * (2.1f);
        float minZ = GameData.gameAreaWidth * (-2.1f);
        float maxZ = GameData.gameAreaHeight * (2.1f);
        
        var point = new Vector3(
            Random.Range(minX, maxX),
            0,
            Random.Range(minZ, maxZ));

        return point;
    }
    
    public GameObject BuildRing(RingColor color)
    {
        GameObject prefab = Resources.Load<GameObject>(UnitType(color));
        RingModel model = prefab.GetComponentInChildren<RingModel>();

        model.unitDestroyRadius = GameData.unitDestroyRadius;
        model.unitSpeed = Random.Range(GameData.unitSpawnMinSpeed, GameData.unitSpawnMaxSpeed);
        
        return prefab;
    }
    private string UnitType(RingColor color)
    {
        string typePrefab = "BlueRing";
        if (color == RingColor.blue) typePrefab = "BlueRing";
        else if (color == RingColor.red) typePrefab = "RedRing";
        return typePrefab;
    }
    
    public GameObject BuildDummy()
    {
        GameObject prefab = Resources.Load<GameObject>("dummy");
        return prefab;
    }
    
    public GameObject BuildBattleField(int width, int height)
    {
        GameObject prefab = Resources.Load<GameObject>("BattleField");
        BattleFieldModel p = prefab.GetComponent<BattleFieldModel>();
        p.areaWidth = width;
        p.areaHeight = height;
        return prefab;
    }

    private void HideDummys()
    {
        foreach (var doomy in poolDummys)
            doomy.SetActive(false); 
    }
    
    private void VisibleDummys()
    {
        foreach (var doomy in poolDummys)
            doomy.SetActive(true); 
    }
}