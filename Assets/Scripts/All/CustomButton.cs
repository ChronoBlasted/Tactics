using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Chrono.UI
{
    public class CustomButton : Button
    {
        enum ButtonType { NONE, SCALE }

        [SerializeField] ButtonType _type;
        [SerializeField] RectTransform _rt;
        [SerializeField] float _timeOfScale = .2f;

        Tweener typeTween;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            switch (_type)
            {
                case ButtonType.SCALE:
                    ScaleDownElement();
                    break;
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            switch (_type)
            {
                case ButtonType.SCALE:
                    ResetScaleElement();
                    break;
            }
        }

        private void ScaleDownElement()
        {
            if (typeTween.IsActive()) typeTween.Kill(true);
            typeTween = _rt.transform.DOScale(new Vector3(.95f, .95f, .95f), _timeOfScale).SetEase(Ease.OutExpo).SetUpdate(UpdateType.Normal, true);
        }
        private void ResetScaleElement()
        {
            if (typeTween.IsActive()) typeTween.Kill(true);
            typeTween = _rt.transform.DOScale(Vector3.one, _timeOfScale).SetEase(Ease.OutExpo).SetUpdate(UpdateType.Normal, true);
        }
    }
}