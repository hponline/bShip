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

    [Header("Obstackles")]
    public Transform obstackleSpawnParent;

    [Header("Spawn Point")]
    public Transform coinSpawnParent;

    Queue<GameObject> mapPool = new();
    List<GameObject> activeMaps = new();

    [Header("Map Variables")]
    public LevelData[] levels;
    public int prefabIndex;
    float startZ;
    public int poolSize = 5;
    float nextSpawnZ = 0f;
    const float mapSpawnOffsetY = 11.77f;
    const float roadLength = 682f;
    const float spawnDistance = 682; // oyuncudan uzaða spawn etmesi için
    const float despawnSpawnZ = 750f; // oyuncunun arkasýndaki mapler yok olur


    private void Awake()
    {
        startZ = boat.position.z;

        for (int i = 0; i < poolSize; i++) // Mapler oluþturuldu
        {
            GameObject obj = Instantiate(mapPrefabs[prefabIndex], Vector3.zero, Quaternion.identity, mapsParent.transform);

            obj.SetActive(false);
            mapPool.Enqueue(obj);
        }

        for (int i = 0; i < poolSize; i++)
        {
            CreateRoad();
        }
    }

    private void Update()
    {
        float traveled = boat.position.z - startZ;
        if (boat.position.z > nextSpawnZ - roadLength)
        {
            CreateRoad();
            Destroy(firstMap);
        }
        RecycleOldMap(traveled);
    }

    void CreateRoad() // Map üretme
    {
        float traveled = boat.position.z - startZ;
        prefabIndex = ChooseMapPrefabIndex(traveled);
        GameObject mapPart = GetPooledObject(prefabIndex);

        mapPart.transform.position = new Vector3(0, mapSpawnOffsetY, nextSpawnZ + spawnDistance);
        mapPart.SetActive(true);

        mapPart.GetComponent<ResetCoin>()?.ResetCoinSpawn(); // Coin Reset

        nextSpawnZ += roadLength;
        activeMaps.Add(mapPart);
    }

    void RecycleOldMap(float traveled) // Map Geri Dönüþüm
    {
        if (activeMaps.Count == 0) return;

        GameObject firstMap = activeMaps[prefabIndex];
        if (boat.position.z - firstMap.transform.position.z > despawnSpawnZ)
        {
            activeMaps.RemoveAt(0);
            firstMap.SetActive(false);
            mapPool.Enqueue(firstMap);
        }
    }

    int ChooseMapPrefabIndex(float traveled)
    {
        foreach (var lvl in levels)
        {
            if (traveled < lvl.distanceThreshold)
            {
                return Random.Range(lvl.minPrefabIndex, lvl.maxPrefabIndex);
            }
        }
        var last = levels[levels.Length - 1];
        return Random.Range(last.minPrefabIndex, last.maxPrefabIndex);
    }

    GameObject GetPooledObject(float traveled) // Havuzdan Map çekme
    {
        if (mapPool.Count > 0)
        {
            GameObject obj = mapPool.Dequeue();
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(mapPrefabs[prefabIndex]);
            return obj;
        }
    }

}

// Havuz dolu oldugu için yeni üretilen mapleri üzerine eklemiyor