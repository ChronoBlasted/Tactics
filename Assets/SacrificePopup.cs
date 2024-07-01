using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SacrificePopup : Popup
{
    [SerializeField] TMP_Text sacrificeTxt;

    int _amountOfSacrifice;

    ACard cardToPlay;

    public override void Init()
    {
        base.Init();
    }

    public void UpdateData(int amountOfSacrifice, ACard card)
    {
        _amountOfSacrifice = amountOfSacrifice;
        cardToPlay = card;

        sacrificeTxt.text = "Choose " + _amountOfSacrifice + " card(s) to sacrifice !";
    }

    public override void OpenPopup()
    {
        base.OpenPopup();

        var allCard = MatchManager.Instance.Board.playerCardBench;
        var allCardHand = DeckManager.Instance.playerCardHolder.cards;

        foreach (var card in allCard)
        {
            card.CardRenderer.SetOnSacrificeMode();
        }

        foreach (var card in allCardHand)
        {
            card.CardRenderer.canInteract = false;
        }
    }

    public override void ClosePopup()
    {
        base.ClosePopup();

        var allCard = MatchManager.Instance.Board.playerCardBench;
        var allCardHand = DeckManager.Instance.playerCardHolder.cards;

        foreach (var card in allCard)
        {
            card.CardRenderer.SetOffSacrificeMode();
        }

        foreach (var card in allCardHand)
        {
            card.CardRenderer.canInteract = true;
        }
    }

    public void Valid()
    {
        var allCard = MatchManager.Instance.Board.playerCardBench;
        var countSelected = 0;

        List<ACard> sacrificeCards = new List<ACard>();

        foreach (var card in allCard)
        {
            if (card.CardRenderer.isSelected)
            {
                countSelected++;
                sacrificeCards.Add(card);
            }
        }

        if (countSelected >= _amountOfSacrifice)
        {
            ClosePopup();

            MatchManager.Instance.Board.SpawnCard(MatchManager.Instance.isPlayerTurn, cardToPlay);

            cardToPlay.PlayCard();

            foreach (ACard card in sacrificeCards)
            {
                card.Sacrifice();
            }
        }
        else
        {
            UIManager.Instance.DoFloatingText("You need to select " + _amountOfSacrifice + " card(s) to play this card", Color.red);
        }
    }
}
