using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    [SerializeField] ACard cardPrefab;

    public void AddCard(EntityData entityData)
    {
        var card = Instantiate(cardPrefab, transform.position, transform.rotation, transform);

        card.EntityData = entityData;

        card.Init();
    }
}
