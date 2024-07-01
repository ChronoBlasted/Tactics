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
    [SerializeField] TMP_Text levelTxt;
    [SerializeField] Image raycastPadding;

    public Canvas canvas;
    public GraphicRaycaster gr;
    public event Action OnBeginDragEvent;
    public event Action OnEndDragEvent;

    bool isDragging;
    bool isSacrificeMode;
    public bool isSelected;
    public bool canInteract = true;
    public bool isInHand = true;

    Tween MoveUpTween;

    public void Init(EntityData newData)
    {
        data = newData;

        visual.sprite = data.visual;

        nameTxt.text = data.name;
        levelTxt.text = data.level.ToString();
        descTxt.text = data.description;
        attackTxt.text = data.attack.ToString();
        heatlhTxt.text = data.maxHealth.ToString();
    }

    #region Drag

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isInHand)
        {
            isDragging = true;

            OnBeginDragEvent?.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isInHand)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isInHand)
        {
            if (transform.position.y > 400)
            {
                isInHand = false;
                raycastPadding.enabled = false;

                var data = new object[] { card };

                GameEventSystem.Instance.Send(EventType.PLAYCARD, data);
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }

            OnEndDragEvent?.Invoke();

            isDragging = false;
        }
    }

    #endregion

    #region Pointer

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isInHand && !isDragging && canInteract)
        {
            MoveUpTween = transform.DOLocalMoveY(310, .2f);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isInHand && !isDragging && canInteract)
        {
            MoveUpTween = transform.DOLocalMoveY(0, .2f);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MoveUpTween.Kill();

        if (isSacrificeMode)
        {
            isSelected = !isSelected;

            ToggleSelected(isSelected);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        MoveUpTween.Kill();
    }

    #endregion

    public void ResetCardInHand()
    {
        isInHand = true;
        raycastPadding.enabled = true;
        transform.localPosition = Vector3.zero;
    }

    public void SetOnSacrificeMode()
    {
        isSacrificeMode = true;
        canvas.sortingOrder = 10;

        MoveUpTween = transform.DOLocalMoveY(256, .2f);
    }

    public void SetOffSacrificeMode()
    {
        isSacrificeMode = false;
        canvas.sortingOrder = 1;

        isSelected = false;
        ToggleSelected(isSelected);

        MoveUpTween = transform.DOLocalMoveY(0, .2f);
    }

    public void ToggleSelected(bool setSelect)
    {
        if (setSelect)
        {
            background.color = Color.yellow;
        }
        else
        {
            background.color = Color.white;
        }
    }
}
