using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NpcThrowObstacle : MonoBehaviour
{
    public GameObject throwGameObjectPrefab;
    public GameObject ObstaclePrefab;
    public Transform startPos;
    public Transform endPos;
    Coroutine throwCoroutine;

    public void StartThrowing()
    {
        if (throwCoroutine == null)
            throwCoroutine = StartCoroutine(ThrowLoop());
    }

    IEnumerator ThrowLoop()
    {
        while (true)
        {
            yield return StartCoroutine(NpcThrowObject(1f));
        }
    }

    public IEnumerator NpcThrowObject(float delay)
    {
        GameObject temp = Instantiate(throwGameObjectPrefab, startPos.position, Quaternion.identity);
        temp.transform.localScale = Vector3.zero;
        temp.transform.DOScale(Vector3.one * 5f, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(1f); // Atýþlar arasý süre

        temp.transform.DOMove(endPos.position, 1f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                Instantiate(ObstaclePrefab, endPos.position, Quaternion.identity);
                Destroy(temp);
            });

        yield return new WaitForSeconds(1f + delay); // Atýþtan sonraki bekleme süresi
    }
}
