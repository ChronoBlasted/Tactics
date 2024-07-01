using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    [SerializeField] ACard cardPrefab;

    public List<ACard> cards;
    public CanvasGroup cg;
    public Transform spawnTransform;

    public void AddCard(EntityData entityData)
    {
        var card = Instantiate(cardPrefab, transform.position, transform.rotation, transform);

        card.EntityData = entityData;

        card.Init();

        card.CardRenderer.transform.position = spawnTransform.position;
        card.CardRenderer.transform.DOLocalMove(Vector3.zero, .5f).SetEase(Ease.OutSine);

        cards.Add(card);
    }
}
