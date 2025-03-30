using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] mapPrefabs;
    public Transform boat;

    Queue<GameObject> mapPool = new Queue<GameObject>();
    List<GameObject> activeMaps = new List<GameObject>();

    public int poolSize = 5;
    float roadLength = 684f;
    float nextSpawnZ = 0f;
    public float despawnSpawnZ = 750f;

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++) // Mapler oluþturuldu
        {
            GameObject obj = Instantiate(mapPrefabs[0], Vector3.zero, Quaternion.identity);
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
        if (boat.position.z > nextSpawnZ -roadLength)        
            CreateRoad();

        RecycleOldMap();
    }

    void CreateRoad() // Map üretme
    {
        GameObject mapPart = GetPooledObject();
        mapPart.transform.position = new Vector3(0, 11.77f, nextSpawnZ);
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
