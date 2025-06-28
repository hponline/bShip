using UnityEngine;


public class BoatObstackleSpawn : MonoBehaviour
{
    [Header("Boat Setup")]
    public GameObject[] npcBoat;
    public Transform[] spawnPoint;
    public Transform[] startPos;
    public Transform[] endPos;
    public float boatSpeed;
    bool npcSpawnerActive = false;

    [Header("Obstacle")]
    public GameObject obstackleSpawn;

    NpcThrowObstacle[] npcThrowScript;

    private void OnEnable()
    {
        #region Engel fýrlatma

        npcThrowScript = new NpcThrowObstacle[npcBoat.Length];
        for (int i = 0; i < npcBoat.Length; i++)
        {
            npcThrowScript[i] = npcBoat[i].GetComponentInChildren<NpcThrowObstacle>();
        }
        #endregion

        npcSpawnerActive = false;

        for (int i = 0; i < npcBoat.Length; i++)        
            npcBoat[i].transform.position = startPos[i].position;        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcSpawnerActive = true;

            foreach (var script in npcThrowScript)
            {
                script?.StartThrowing();
            }
        }
    }
    private void Update()
    {
        if (npcSpawnerActive)
        {
            MoveBoatsToTarget();
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
            GameObject boat = npcBoat[i];
            Vector3 target = endPos[i].transform.position;

            boat.transform.position = Vector3.MoveTowards(boat.transform.position, target, boatSpeed * Time.deltaTime);

            float distance = Vector3.Distance(boat.transform.position, target);
            if (distance < 30f)
            {
                StopObstacleSpawning(); // Spawn Durdur

                BoatScale fade = boat.GetComponentInChildren<BoatScale>();
                if (fade != null)
                    fade.FadeOutAndDisable(2f);
            }
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
        foreach (var script in npcThrowScript)
        {
            script?.StopThrowing();
        }
    }

    public void ResetBoatsToStart()
    {
        for (int i = 0; i < npcBoat.Length; i++)
        {
            npcBoat[i].transform.position = startPos[i].position;
        }
    }
}