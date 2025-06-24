using UnityEngine;
using DG.Tweening;

public class BoatScale : MonoBehaviour
{
    bool isFading = false;
    public void FadeOutAndDisable(float duration = 0.8f)
    {
        if (isFading) return;
        isFading = true;

        Debug.Log("Fade baþlýyor");

        transform.DOScale(Vector3.zero, duration).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            Debug.Log("Fade bitti ve obje kapatýlýyor");
            //gameObject.SetActive(false);
            isFading = false;
        });
    }
}
