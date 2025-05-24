using System.Collections;
using UnityEngine;

public class boatObstackleSpawn : MonoBehaviour
{
    public GameObject[] npcBoat;
    public float boatSpeed;
    public Transform[] spawnPoint;
    public float[] spawnDelay;
    public Transform[] startPos;
    public Transform[] endPos;
    public GameObject obstackleSpawn;
    bool npcSpawnerActive = false;

    Coroutine[] spawnCoroutines;

    private void Start()
    {
        spawnCoroutines = new Coroutine[startPos.Length];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcSpawnerActive = true;
        }
    }
    private void Update()
    {
        if (npcSpawnerActive)
        {
            MoveBoatsToTarget();
            StartObstacleSpawning();
        }

        if (npcSpawnerActive && AllBoatsReachedTarget())
        {
            npcSpawnerActive = false;
            ResetBoatsToStart();
        }
    }

    public void MoveBoatsToTarget()
    {
        for (int i = 0; i < startPos.Length; i++)
        {
            npcBoat[i].transform.position = Vector3.MoveTowards(npcBoat[i].transform.position, endPos[i].transform.position, boatSpeed * Time.deltaTime);
        }
    }

    public bool AllBoatsReachedTarget()
    {
        for (int i = 0; i < npcBoat.Length; i++)
        {
            if (Vector3.Distance(npcBoat[i].transform.position, endPos[i].position) > 1f)
                return false;
        }
        return true;
    }

    public void ResetBoatsToStart()
    {
        for (int i = 0; i < npcBoat.Length; i++)
        {
            npcBoat[i].transform.position = startPos[i].position;
        }
    }

    public void StartObstacleSpawning()
    {
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            if (spawnCoroutines[i] == null)
            {
                spawnCoroutines[i] = StartCoroutine(NpcObstackleSpawner(i, spawnDelay[i]));
            }
        }
    }

    IEnumerator NpcObstackleSpawner(int index, float delay)
    {
        while (true)
        {
            GameObject temp = Instantiate(obstackleSpawn, spawnPoint[index].transform.position, Quaternion.identity);

            Destroy(temp, 5);
            yield return new WaitForSeconds(delay);
        }
    }
}
// Animasyon ile engellerin büyüyüp küçülmesi yapýlacak