using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstacklePrefabs;

    public Transform[] coinSpawnPoints;
    public GameObject coinPrefabs;

    public GameManager gameManager;

    float boatSpawnDistance = 300f;
    public float coinSpawnDistance = 500f;

    private float spawnLeft = -75.7f;
    private float spawnRight = 31;
   

    // 2 saniye sonra baslar. 1 saniye de bir dongu tekrarlanir.
    private float startDelay = 2;
    private float startInterval = 3.05f;

    void Start()
    {
        InvokeRepeating("SpawnRandomObstackle", startDelay, startInterval);
        InvokeRepeating("SpawnRandomCoin", startDelay, startInterval);
    }

    void Update()
    {
        
    }

    void SpawnRandomObstackle()
    {
        Vector3 spawnOffSet = new Vector3(0, 0 , boatSpawnDistance);
        int obstackleIndex = Random.Range(0, obstacklePrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(spawnLeft, spawnRight), 0, gameManager.boat.position.z + spawnOffSet.z);

        Instantiate(obstacklePrefabs[obstackleIndex], spawnPos, obstacklePrefabs[obstackleIndex].transform.rotation);
    }

    void SpawnRandomCoin()
    {
        Vector3 spawnOffSet = new Vector3(0, 0, coinSpawnDistance);
        int spawnPosIndex = Random.Range(0, coinSpawnPoints.Length);        
        Vector3 spawnPos = new Vector3(coinSpawnPoints[spawnPosIndex].position.x, 10f, gameManager.boat.position.z + spawnOffSet.z);
                
        Instantiate(coinPrefabs, spawnPos, Quaternion.identity);
    }
}
