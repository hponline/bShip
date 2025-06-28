using System.Collections;
using UnityEngine;
using DG.Tweening;

public class NpcThrowObstacle : MonoBehaviour
{
    //public GameObject throwGameObjectPrefab;
    public GameObject obstaclePrefab;
    public Transform startPos;
    public Transform targetPos;
    float[] obstacleTime = { 2f, 4f, 6f };

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
            int randomIndex = Random.Range(0, obstacleTime.Length);
            float currentDelay = obstacleTime[randomIndex];
            yield return StartCoroutine(NpcThrowObject(currentDelay, 2f));
        }
    }

    public IEnumerator NpcThrowObject(float time, float delay)
    {
        // Fýrlatýlan obje
        GameObject temp = Instantiate(obstaclePrefab, startPos.position, Quaternion.identity);
        temp.transform.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();
        seq.Append(temp.transform.DOScale(Vector3.one * 3f, 0.7f).SetEase(Ease.OutBack));
        seq.Join(temp.transform.DOMove(targetPos.position, 0.6f).SetEase(Ease.InOutQuad));
        seq.Append(temp.transform.DOScale(Vector3.one * 9f, 0.5f).SetEase(Ease.OutBack));

        yield return new WaitForSeconds(time); // Obstacle sahnede kalýr

        // Obstacle Destroy
        if (temp != null)
        {
            temp.transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                DOTween.Kill(temp.transform);
                Destroy(temp);
            });
        }

        yield return new WaitForSeconds(delay); // Atýþtan sonraki bekleme süresi
    }
}
