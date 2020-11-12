using System.IO;
using UnityEngine;

public class Preloader : MonoBehaviour
{
    private void Awake()
    {
        TextAsset txtAsset = (TextAsset)Resources.Load("GameConfig", typeof(TextAsset));
        string json = txtAsset.text;
        GameProperties loadedData = JsonUtility.FromJson<GameProperties>(json);

        GameData.gameAreaWidth = loadedData.gameAreaHeight;
        GameData.gameAreaHeight = loadedData.gameAreaHeight;
        GameData.numUnitsToSpawn = loadedData.numUnitsToSpawn;
        GameData.unitSpawnDelay = loadedData.unitSpawnDelay;
        GameData.unitSpawnMinRadius = loadedData.unitSpawnMinRadius;
        GameData.unitSpawnMaxRadius = loadedData.unitSpawnMaxRadius;
        GameData.unitSpawnMinSpeed = loadedData.unitSpawnMinSpeed;
        GameData.unitSpawnMaxSpeed = loadedData.unitSpawnMaxSpeed;
        GameData.unitDestroyRadius = loadedData.unitDestroyRadius;
        GameData.unitDeflateStep = loadedData.unitDeflateStep;
    }

    class GameProperties
    {
        public int gameAreaWidth;
        public int gameAreaHeight;
        public int numUnitsToSpawn;
        public float unitSpawnDelay;
        public float unitSpawnMinRadius;
        public float unitSpawnMaxRadius;
        public float unitSpawnMinSpeed;
        public float unitSpawnMaxSpeed;
        public float unitDestroyRadius;
        public float unitDeflateStep;
    }
}
