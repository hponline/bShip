using System.Collections;
using UnityEngine;
using DG.Tweening;

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

    private void OnEnable()
    {
        if (spawnCoroutines == null || spawnCoroutines.Length != startPos.Length)
            spawnCoroutines = new Coroutine[startPos.Length];

        npcSpawnerActive = false;

        for (int i = 0; i < npcBoat.Length; i++)
        {
            npcBoat[i].transform.position = startPos[i].position;
        }
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
            StopObstacleSpawning();
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

    public void StopObstacleSpawning()
    {
        if (spawnCoroutines == null) return;

        for (int i = 0; i < spawnCoroutines.Length; i++)
        {
            if (spawnCoroutines != null)
            {
                StopCoroutine(spawnCoroutines[i]);
                spawnCoroutines[i] = null;
            }
        }
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
            temp.transform.localScale = Vector3.zero;
            temp.transform.DOScale(Vector3.one * 9f, 0.5f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(4.5f);

            temp.transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                Destroy(temp);
            });

            yield return new WaitForSeconds(0.5f + delay);
        }
    }
}