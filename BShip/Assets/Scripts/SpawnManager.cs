using System.Collections.Generic;
using UnityEngine;

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
    public int poolSize = 5;
    float nextSpawnZ = 0f;
    const float mapSpawnOffsetY = 11.77f;
    const float roadLength = 682f;
    const float spawnDistance = 682; // oyuncudan uzaða spawn etmesi için
    const float despawnSpawnZ = 750f; // oyuncunun arkasýndaki mapler yok olur


    public int sayac;

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++) // Mapler oluþturuldu
        {
            GameObject obj = Instantiate(mapPrefabs[Random.Range(0, mapPrefabs.Length)], Vector3.zero, Quaternion.identity, mapsParent.transform);

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
        if (boat.position.z > nextSpawnZ - roadLength)
        {
            CreateRoad();
            Destroy(firstMap);
        }
        RecycleOldMap();
    }

    void CreateRoad() // Map üretme
    {
        GameObject mapPart = GetPooledObject();
        mapPart.transform.position = new Vector3(0, mapSpawnOffsetY, nextSpawnZ + spawnDistance);
        mapPart.SetActive(true);

        if (mapPart != null) // Coin Reset
        {
            mapPart.GetComponent<ResetCoin>().ResetCoinSpawn();
        }
        nextSpawnZ += roadLength;
        activeMaps.Add(mapPart);
    }

    void RecycleOldMap() // Map Geri Dönüþüm
    {
        if (activeMaps.Count > 0)
        {
            GameObject firstMap = activeMaps[0];

            if (boat.position.z - firstMap.transform.position.z > despawnSpawnZ)
            {
                activeMaps.RemoveAt(0);
                firstMap.SetActive(false);
                mapPool.Enqueue(firstMap);
            }
        }
    }

    GameObject GetPooledObject() // Havuzdan Map çekme
    {
        if (mapPool.Count > 0)
        {
            GameObject obj = mapPool.Dequeue();
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(mapPrefabs[Random.Range(0, mapPrefabs.Length)]);
            return obj;
        }
    }
}
