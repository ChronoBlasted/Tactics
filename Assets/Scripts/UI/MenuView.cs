using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuView : View
{
    [SerializeField] List<ACard> cards = new List<ACard>();

    public override void Init()
    {
        base.Init();

        foreach (var card in cards)
        {
            card.EntityData = DeckManager.Instance.GetRandomCard(DeckManager.Instance.allCard);
            card.Init(true);
        }
    }
}
