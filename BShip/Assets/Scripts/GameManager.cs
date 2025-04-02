using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManagerInstance;

    public GameObject[] mapPrefabs;
    public Transform mapsParent;
    public Transform boat;

    [Header("UI")]
    public TextMeshProUGUI coinText;

    Queue<GameObject> mapPool = new();
    List<GameObject> activeMaps = new();

    [Header("Variables")]
    public int poolSize = 3;
    float nextSpawnZ = 0f;
    const float roadLength = 682f;
    const float spawnDistance = 682; // oyuncudan uzaða spawn etmesi için
    const float despawnSpawnZ = 750f;

    private void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


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

    private void Update()
    {
        if (boat.position.z > nextSpawnZ - roadLength)
            CreateRoad();

        RecycleOldMap();
    }

    void CreateRoad() // Map üretme
    {
        GameObject mapPart = GetPooledObject();
        mapPart.transform.position = new Vector3(0, 11.77f, nextSpawnZ + spawnDistance);
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
