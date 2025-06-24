using UnityEngine;
using DG.Tweening;

public class BoatScale : MonoBehaviour
{
    bool isFading = false;
    public void FadeOutAndDisable(float duration = 0.8f)
    {
        if (isFading) return;
        isFading = true;

        Debug.Log("Fade ba�l�yor");

        transform.DOScale(Vector3.zero, duration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            Debug.Log("Fade bitti ve obje kapat�l�yor");
            //gameObject.SetActive(false);
            isFading = false;
        });
    }
}
