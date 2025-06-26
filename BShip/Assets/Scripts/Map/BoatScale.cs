using UnityEngine;
using DG.Tweening;

public class BoatScale : MonoBehaviour
{
    Vector3 originalScale;
    bool isFading = false;
    float sinkDistance = 20f;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void FadeOutAndDisable(float duration = 0.8f)
    {
        if (isFading) return;
        isFading = true;

        float sinkY = transform.position.y - sinkDistance;

        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Join(transform.DOScale(Vector3.zero, duration).SetEase(Ease.InOutSine));
        fadeSequence.Join(transform.DOMoveY(sinkY, duration).SetEase(Ease.InOutSine));
        fadeSequence.OnComplete(() =>
        {
            isFading = false;
        });
    }

    private void OnEnable()
    {
        transform.localScale = originalScale;
        transform.DOKill();
        isFading = false;
    }
}
