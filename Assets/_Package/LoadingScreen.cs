using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public void Show()
    {
        canvasGroup.alpha = 0;
        this.gameObject.SetActive(true);

        canvasGroup.DOKill();
        canvasGroup.DOFade(1, 0.5f);
    }

    public void Hide()
    {
        canvasGroup.alpha = 1;

        canvasGroup.DOKill();
        canvasGroup.DOFade(0, 0.5f).OnComplete(() => this.gameObject.SetActive(false));
    }
}
