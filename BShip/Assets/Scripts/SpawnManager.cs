using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Obstackles")]
    public GameObject[] obstacklePrefabs;
    public Transform obstackleSpawnParent;

    [Header("Spawn Point")]
    public Transform[] coinSpawnPoints;
    public Transform coinSpawnParent;

    [Header("CoinPrefab")]
    public GameObject coinPrefabs;

    [Header("Variables")]
    const float boatObstackleDistance = 300f;
    const float coinSpawnDistance = 500f;
    const float spawnLeft = -75.7f;
    const float spawnRight = 31;
    const float startDelay = 2;
    const float startInterval = 3.05f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnRandomObstackle), startDelay, startInterval);
        InvokeRepeating(nameof(SpawnRandomCoin), startDelay, startInterval);
    }


    void SpawnRandomObstackle()
    {
        Vector3 spawnOffSet = new (0, 0, boatObstackleDistance);
        int obstackleIndex = Random.Range(0, obstacklePrefabs.Length);
        Vector3 spawnPos = new (Random.Range(spawnLeft, spawnRight), 0, gameManager.boat.position.z + spawnOffSet.z);

        Instantiate(obstacklePrefabs[obstackleIndex], spawnPos, obstacklePrefabs[obstackleIndex].transform.rotation, obstackleSpawnParent.transform);
    }

    void SpawnRandomCoin()
    {
        Vector3 spawnOffSet = new (0, 0, coinSpawnDistance);
        int spawnPosIndex = Random.Range(0, coinSpawnPoints.Length);
        Vector3 spawnPos = new (coinSpawnPoints[spawnPosIndex].position.x, 10f, gameManager.boat.position.z + spawnOffSet.z);

        Instantiate(coinPrefabs, spawnPos, Quaternion.identity, coinSpawnParent.transform);
    }
}
