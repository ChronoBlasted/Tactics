using DG.Tweening;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    [SerializeField] protected CanvasGroup _canvasGroup;

    public virtual void Init()
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0f;
    }

    public virtual void OpenView(float duration = .2f)
    {
        _canvasGroup.DOFade(1, duration).OnComplete
        (() =>
        {
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }).SetUpdate(UpdateType.Normal, true);
    }

    public virtual void CloseView(float duration = .2f)
    {
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;

        _canvasGroup.DOFade(0, duration).SetUpdate(UpdateType.Normal, true);
    }
}