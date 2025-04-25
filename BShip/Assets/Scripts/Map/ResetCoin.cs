using UnityEngine;

public class ResetCoin : MonoBehaviour
{
    private const string SpawnPointTag = "Coin";

    public Transform[] GetBranchSpawnPoints()
    {
        var pts = GetComponentsInChildren<Transform>(true);
        return System.Array.FindAll(pts, t => t.CompareTag(SpawnPointTag));
    }

    public void ResetCoinSpawn()
    {
        foreach (var coin in GetComponentsInChildren<CoinTurn>(true))
            coin.gameObject.SetActive(true);
    }

}
