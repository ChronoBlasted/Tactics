using System.Collections.Generic;
using TMPro;
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

        var allCard = cardToPlay.isPlayerCard ? MatchManager.Instance.Board.playerCardBench : MatchManager.Instance.Board.opponentCardBench;
        var allCardHand = cardToPlay.isPlayerCard ? DeckManager.Instance.playerCardHolder.cards : DeckManager.Instance.opponentCardHolder.cards;

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

        var allCard = cardToPlay.isPlayerCard ? MatchManager.Instance.Board.playerCardBench : MatchManager.Instance.Board.opponentCardBench;
        var allCardHand = cardToPlay.isPlayerCard ? DeckManager.Instance.playerCardHolder.cards : DeckManager.Instance.opponentCardHolder.cards;

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
        var allCard = cardToPlay.isPlayerCard ? MatchManager.Instance.Board.playerCardBench : MatchManager.Instance.Board.opponentCardBench;
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

            foreach (ACard card in sacrificeCards)
            {
                card.Sacrifice();
            }

            MatchManager.Instance.Board.DoSpawnCard(MatchManager.Instance.isPlayerTurn, cardToPlay);

            cardToPlay.PlayCard();
        }
        else
        {
            UIManager.Instance.DoFloatingText("You need to select " + _amountOfSacrifice + " card(s) to play this card", Color.red);
        }
    }

    public void HandleReturn()
    {
        cardToPlay.CardRenderer.ResetCardInHand();

        ClosePopup();
    }
}
