using System.Collections;
using UnityEngine;

public class boatObstackleSpawn : MonoBehaviour
{
    public GameObject[] npcBoat;
    public float boatSpeed;
    public Transform spawnPoint;
    public Transform[] startPos;
    public Transform[] endPos;
    public GameObject obstackleSpawn;
    bool npcSpawnerActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcSpawnerActive = true;
            StartCoroutine(NpcObstackleSpawner());

        }
    }
    private void Update()
    {
        if (npcSpawnerActive)
        {
            for (int i = 0; i < startPos.Length; i++)
            {
                npcBoat[i].transform.position = Vector3.MoveTowards(npcBoat[i].transform.position, endPos[i].transform.position, boatSpeed * Time.deltaTime);
            }

        }

        bool allReached = true;
        for (int i = 0; i < npcBoat.Length; i++)
        {
            if (Vector3.Distance(npcBoat[i].transform.position, endPos[i].position) > 0.1f)
            {
                allReached = false;
                break;
            }
        }

        if (allReached)
        {
            npcSpawnerActive = false;

            for (int i = 0; i < npcBoat.Length; i++)
            {
                npcBoat[i].transform.position = startPos[i].position;
            }
        }


        //if (npcBoat.transform.position == endPos.position)
        //{
        //    npcSpawnerActive = false;
        //    for (int i = 0; i < startPos.Length; i++)
        //    {
        //        npcBoat[i].transform.position = startPos[i].position;
        //    }
        //}
    }

    IEnumerator NpcObstackleSpawner()
    {
        while (true)
        {

            GameObject temp = Instantiate(obstackleSpawn, spawnPoint.transform.position, Quaternion.identity);

            Destroy(temp, 5);
            yield return new WaitForSeconds(3);
        }
    }

}  
// Tekneler iç içe geçiyor, sadece biri spawn ediyor düzelt
