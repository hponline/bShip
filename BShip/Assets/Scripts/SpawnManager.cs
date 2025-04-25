using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public float distanceThreshold;
    public int minPrefabIndex;
    public int maxPrefabIndex;
}

public class SpawnManager : MonoBehaviour
{
    [Header("Map Spawn")]
    public GameObject[] mapPrefabs;
    public GameObject firstMap;
    public Transform mapsParent;
    public Transform boat;

    [Header("Ship")]
    public ShipController ship;

    Queue<GameObject> mapPool = new();
    List<GameObject> activeMaps = new();
    List<int> allowedPrefab = new();

    [Header("Map Variables")]
    public int poolSize = 5;
    public LevelData[] levels;
    float startZ;
    float nextSpawnZ = 0f;
    const float mapSpawnOffsetY = 11.77f;
    const float roadLength = 682f;
    const float spawnDistance = 682; // oyuncudan uzaða spawn etmesi için
    const float despawnSpawnZ = 750f; // oyuncunun arkasýndaki mapler yok olur
    int currentLevel = 0;


    private void Awake()
    {
        startZ = boat.position.z;

        var lvl0 = levels[0];
        for (int i = lvl0.minPrefabIndex; i < lvl0.maxPrefabIndex; i++)
            allowedPrefab.Add(i);

        for (int i = 0; i < poolSize; i++) // Mapler oluþturuldu
        {
            int index = Random.Range(lvl0.minPrefabIndex, lvl0.maxPrefabIndex);
            GameObject obj = Instantiate(mapPrefabs[index], Vector3.zero, Quaternion.identity, mapsParent.transform);

            obj.SetActive(false);
            mapPool.Enqueue(obj);
        }

        for (int i = 0; i < poolSize; i++)
            CreateRoad();        
    }

    private void Update()
    {
        float traveled = boat.position.z - startZ;

        while (currentLevel + 1 < levels.Length && traveled >= levels[currentLevel + 1].distanceThreshold)
        {
            currentLevel++;
            AddLevel(currentLevel);
            ship.moveSpeed += 15;
        }

        if (boat.position.z > nextSpawnZ - roadLength)
        {
            CreateRoad();
            Destroy(firstMap); // Baþlangýç Zemini
        }
        RecycleOldMap();
    }

    void AddLevel(int lvl)
    {
        var data = levels[lvl];
        for (int i = data.minPrefabIndex; i < data.maxPrefabIndex; i++)        
            allowedPrefab.Add(i);

        for (int i = 0; i < poolSize; i++)
        {
            int index = Random.Range(data.minPrefabIndex, data.maxPrefabIndex);
            GameObject obj = Instantiate(mapPrefabs[index], Vector3.zero, Quaternion.identity, mapsParent.transform);
            obj.SetActive(false);
            mapPool.Enqueue(obj);
        }        
    }

    void CreateRoad() // Map üretme
    {
        GameObject mapPart = GetPooledObject();
        mapPart.transform.position = new Vector3(0, mapSpawnOffsetY, nextSpawnZ + spawnDistance);
        mapPart.SetActive(true);

        mapPart.GetComponent<ResetCoin>()?.ResetCoinSpawn(); // Coin Reset

        nextSpawnZ += roadLength;
        activeMaps.Add(mapPart);
    }

    void RecycleOldMap() // Map Geri Dönüþüm
    {
        if (activeMaps.Count == 0) return;

        GameObject oldMap = activeMaps[0];
        if (boat.position.z - oldMap.transform.position.z > despawnSpawnZ)
        {
            activeMaps.RemoveAt(0);
            oldMap.SetActive(false);
            mapPool.Enqueue(oldMap);
        }
    }

    GameObject GetPooledObject() // Havuzdan Map çekme
    {
        if (mapPool.Count > 0)
        {
            return mapPool.Dequeue();            
        }
        else
        {
            int index = allowedPrefab[Random.Range(0, allowedPrefab.Count)];
            return Instantiate(mapPrefabs[index], mapsParent.transform);
        }
    }
}