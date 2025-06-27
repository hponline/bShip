using System.Collections;
using UnityEngine;
using DG.Tweening;

public class NpcThrowObstacle : MonoBehaviour
{
    //public GameObject throwGameObjectPrefab;
    public GameObject obstaclePrefab;
    public Transform startPos;
    public Transform targetPos;
    public float[] delay = { 4f, 6f, 8f };

    Coroutine throwCoroutine;

    public void StartThrowing()
    {
        if (throwCoroutine == null)
            throwCoroutine = StartCoroutine(ThrowLoop());
    }

    public void StopThrowing()
    {
        if (throwCoroutine != null)
        {
            StopCoroutine(throwCoroutine);
            throwCoroutine = null;
        }
    }

    IEnumerator ThrowLoop()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, delay.Length);
            Debug.Log(randomIndex);
            yield return StartCoroutine(NpcThrowObject(randomIndex));
        }
    }

    public IEnumerator NpcThrowObject(float delay)
    {
        // Fýrlatýlan obje
        GameObject temp = Instantiate(obstaclePrefab, startPos.position, Quaternion.identity);
        temp.transform.localScale = Vector3.zero;

        temp.transform.DOScale(Vector3.one * 3f, 0.7f).SetEase(Ease.OutBack);
        temp.transform.DOMove(targetPos.position, 0.6f).SetEase(Ease.InOutQuad).OnComplete(() => temp.transform.DOScale(Vector3.one * 9f, 0.5f).SetEase(Ease.OutBack));

        yield return new WaitForSeconds(1f);

        // Obstacle Destroy
        yield return new WaitForSeconds(delay);
        temp.transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() => Destroy(temp));

        yield return new WaitForSeconds(1.5f); // Atýþtan sonraki bekleme süresi
    }
}
