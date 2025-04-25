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
    const float spawnDistance = 682; // oyuncudan uza�a spawn etmesi i�in
    const float despawnSpawnZ = 750f; // oyuncunun arkas�ndaki mapler yok olur


    private void Awake()
    {
        startZ = boat.position.z;

        float traveled = boat.position.z - startZ;
        prefabIndex = ChooseMapPrefabIndex(traveled);
        for (int i = 0; i < poolSize; i++) // Mapler olu�turuldu
        {
            GameObject obj = Instantiate(mapPrefabs[prefabIndex], Vector3.zero, Quaternion.identity, mapsParent.transform);

            obj.SetActive(false);
            mapPool.Enqueue(obj);
        }

        for (int i = 0; i < poolSize; i++)
            CreateRoad();

    }

    private void Update()
    {
        float traveled = boat.position.z - startZ;
        if (boat.position.z > nextSpawnZ - roadLength)
        {
            CreateRoad();
            Destroy(firstMap); // Ba�lang�� Zemini
        }
        RecycleOldMap();
    }

    void CreateRoad() // Map �retme
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

    void RecycleOldMap() // Map Geri D�n���m
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

    int ChooseMapPrefabIndex(float traveled)
    {
        foreach (var lvl in levels)
        {
            if (traveled < lvl.distanceThreshold)
            {
                return Random.Range(lvl.minPrefabIndex, lvl.maxPrefabIndex);
            }
        }
        var last = levels[levels.Length - 1]; // Son haritadan ba�ka yoksa son haritay� d�nd�r
        return Random.Range(last.minPrefabIndex, last.maxPrefabIndex);
    }

    GameObject GetPooledObject(int prefabIndex) // Havuzdan Map �ekme
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

// Havuz dolu oldugu i�in yeni �retilen mapleri �zerine eklemiyor