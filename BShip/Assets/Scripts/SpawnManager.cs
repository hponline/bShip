using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Map Spawn")]
    public GameObject[] mapPrefabs;
    public Transform mapsParent;
    public Transform boat;

    [Header("Obstackles")]
    public GameObject[] obstacklePrefabs;
    public Transform obstackleSpawnParent;
    public float ObstackleDistance = 300f;

    [Header("Spawn Point")]
    public Transform[] coinSpawnPoints;
    public Transform coinSpawnParent;
   
    [Header("Coin Variables")]
    public GameObject coinPrefabs;
    
    public float coinSpawnDistance = 500f;
    public float startDelay = 2;
    public float startInterval = 3.05f;
    const float coinSpawnOffsetY = 10f;
    const float spawnLeft = -75.7f;
    const float spawnRight = 31;

    Queue<GameObject> mapPool = new();
    List<GameObject> activeMaps = new();
    
    [Header("Map Variables")]
    public int poolSize = 5;
    float nextSpawnZ = 0f;
    const float mapSpawnOffsetY = 11.77f;
    const float roadLength = 682f;
    const float spawnDistance = 682; // oyuncudan uzaða spawn etmesi için
    const float despawnSpawnZ = 750f; // oyuncunun arkasýndaki mapler yok olur

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++) // Mapler oluþturuldu
        {
            GameObject obj = Instantiate(mapPrefabs[0], Vector3.zero, Quaternion.identity, mapsParent.transform);

            obj.SetActive(false);
            mapPool.Enqueue(obj);
        }

        for (int i = 0; i < poolSize; i++)
        {
            CreateRoad();
        }
    }


    void Start()
    {
        InvokeRepeating(nameof(SpawnRandomObstackle), startDelay, startInterval);
        InvokeRepeating(nameof(SpawnRandomCoin), startDelay, startInterval);
    }

    private void Update()
    {
        if (boat.position.z > nextSpawnZ - roadLength)
            CreateRoad();

        RecycleOldMap();
    }


    void SpawnRandomObstackle()
    {
        Vector3 spawnOffSet = new (0, 0, ObstackleDistance);
        int obstackleIndex = Random.Range(0, obstacklePrefabs.Length);
        Vector3 spawnPos = new (Random.Range(spawnLeft, spawnRight), 0, boat.position.z + spawnOffSet.z);

        Instantiate(obstacklePrefabs[obstackleIndex], spawnPos, obstacklePrefabs[obstackleIndex].transform.rotation, obstackleSpawnParent.transform);
    }

    void SpawnRandomCoin()
    {
        Vector3 spawnOffSet = new (0, 0, coinSpawnDistance);
        int spawnPosIndex = Random.Range(0, coinSpawnPoints.Length);
        Vector3 spawnPos = new (coinSpawnPoints[spawnPosIndex].position.x, coinSpawnOffsetY, boat.position.z + spawnOffSet.z);

        Instantiate(coinPrefabs, spawnPos, Quaternion.identity, coinSpawnParent.transform);
    }

    void CreateRoad() // Map üretme
    {
        GameObject mapPart = GetPooledObject();
        mapPart.transform.position = new Vector3(0, mapSpawnOffsetY, nextSpawnZ + spawnDistance);
        mapPart.SetActive(true);

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
            GameObject obj = Instantiate(mapPrefabs[0]);
            return obj;
        }
    }
}
