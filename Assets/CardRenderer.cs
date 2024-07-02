using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
    [SerializeField] TMP_Text damageTxt;
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

    public Tween MoveUpTween;

    public void Init(EntityData newData)
    {
        data = newData;

        visual.sprite = data.visual;

        background.sprite = ColorManager.Instance.GetBackgroundByElement(data.element);
        nameTxt.text = data.name;
        levelTxt.text = data.level.ToString();
        descTxt.text = data.description;
        damageTxt.text = data.attack.ToString();
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
            var YtoPlay = card.isPlayerCard ? 400 : -400;

            if (transform.position.y > YtoPlay)
            {
                isInHand = false;

                raycastPadding.enabled = false;

                var data = new object[] { card };

                GameEventSystem.Instance.Send(EventType.PLAYCARD, data);

                canvas.sortingOrder = 1;
                transform.DOScale(Vector3.one, .05f);
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
            MoveUpTween.Kill();

            MoveUpTween = transform.DOLocalMoveY(310, .2f);

            transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), .05f).SetEase(Ease.OutSine);

            canvas.sortingOrder = 3;
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isInHand && !isDragging && canInteract)
        {
            MoveUpTween.Kill();

            transform.DOScale(Vector3.one, .05f);

            MoveUpTween = transform.DOLocalMoveY(0, .2f).OnComplete(() =>
            {
                canvas.sortingOrder = 2;
            });
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
    }

    #endregion

    public void ResetCardInHand()
    {
        isInHand = true;
        raycastPadding.enabled = true;
        transform.localPosition = Vector3.zero;

        canvas.sortingOrder = 2;
        transform.DOScale(Vector3.one, .05f);
    }

    public void SetOnSacrificeMode()
    {
        isSacrificeMode = true;
        canvas.sortingOrder = 10;

        MoveUpTween.Kill();
        MoveUpTween = transform.DOLocalMoveY(256, .2f);
    }

    public void SetOffSacrificeMode()
    {
        isSacrificeMode = false;
        canvas.sortingOrder = 1;

        isSelected = false;
        ToggleSelected(isSelected);

        MoveUpTween.Kill();
        MoveUpTween = transform.DOLocalMoveY(0, .2f);
    }

    public void ToggleSelected(bool setSelect)
    {
        if (setSelect)
        {
            background.color = Color.grey;

            MoveUpTween.Kill();
            MoveUpTween = transform.DOLocalMoveY(376, .2f);
        }
        else
        {
            background.color = Color.white;

            MoveUpTween.Kill();
            MoveUpTween = transform.DOLocalMoveY(256, .2f);
        }
    }

    public void UpdateHealth(int newHealth)
    {
        heatlhTxt.text = newHealth.ToString();
    }

    public void UpdateDamage(int newDamage)
    {
        damageTxt.text = newDamage.ToString();
    }


    private void Update()
    {
        if (!isInHand)
        {
            if (transform.rotation.eulerAngles != Vector3.zero)
                transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
