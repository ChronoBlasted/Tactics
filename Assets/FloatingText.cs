using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public TMP_Text Text;
    Sequence sequenceMoveUp;

    public void Init(string text, Color color)
    {
        Text.text = text;
        Text.color = color;

        if (sequenceMoveUp.IsActive()) sequenceMoveUp.Kill();

        sequenceMoveUp = DOTween.Sequence();

        sequenceMoveUp
            .Join(Text.transform.DOPunchScale(new Vector3(.5f, .5f, .5f), .05f))
            .Append(Text.transform.DOLocalMoveY(64, 1f)).SetEase(Ease.OutSine)
            .Append(Text.DOFade(0, .2f))
            .OnComplete(() => Destroy(gameObject));
    }
}
