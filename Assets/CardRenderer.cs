using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardRenderer : MonoBehaviour,
    IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler,
    IPointerUpHandler, IPointerDownHandler
{
    EntityData data;

    [SerializeField] ACard card;

    [SerializeField] Image background;
    [SerializeField] Image visual;
    [SerializeField] TMP_Text nameTxt;
    [SerializeField] TMP_Text descTxt;
    [SerializeField] TMP_Text attackTxt;
    [SerializeField] TMP_Text heatlhTxt;

    public event Action OnBeginDragEvent;
    public event Action OnEndDragEvent;

    bool isSelected;
    bool isInHand = true;

    Tween HoverTween;

    public void Init(EntityData newData)
    {
        data = newData;

        visual.sprite = data.visual;

        nameTxt.text = data.name;
        descTxt.text = data.description;
        attackTxt.text = data.attack.ToString();
        heatlhTxt.text = data.maxHealth.ToString();
    }

    #region Drag

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke();

        if (transform.position.y > 400 && isInHand)
        {
            isInHand = false;

            var data = new object[] { this };

            GameEventSystem.Instance.Send(EventType.PLAYCARD, data);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }

    #endregion

    #region Pointer

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isInHand)
        {
            HoverTween = transform.DOLocalMoveY(200, .2f);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isInHand)
        {
            HoverTween = transform.DOLocalMoveY(0, .2f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HoverTween.Kill();

        isSelected = true;

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        HoverTween.Kill();

        isSelected = false;
    }
    #endregion
}
