using BaseTemplate.Behaviours;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoSingleton<DeckManager>
{
    [SerializeField] int amountOfCardAtStart = 7;
    [SerializeField] int amountOfCardPerDeck = 20;
    [SerializeField] List<EntityData> allCard = new List<EntityData>();

    [SerializeField] List<EntityData> playerDeck = new List<EntityData>();
    [SerializeField] List<EntityData> opponentDeck = new List<EntityData>();

    public CardHolder playerCardHolder;
    public CardHolder opponentCardHolder;

    EntityData tempCardData;

    public void Init()
    {
        SetupDecks();
        SetupHands();
    }

    public void SetupDecks()
    {
        for (int i = 0; i < amountOfCardPerDeck; i++)
        {
            playerDeck.Add(GetRandomCard(allCard));
            opponentDeck.Add(GetRandomCard(allCard));
        }
    }

    void SetupHands()
    {
        for (int i = 0; i < amountOfCardAtStart; i++)
        {
            DrawCard(true);
            DrawCard(false);
        }
    }
    public void DrawCard(bool isPlayerTurn)
    {
        if (playerDeck.Count > 0 && isPlayerTurn)
        {
            tempCardData = GetRandomCard(playerDeck);
            playerCardHolder.AddCard(tempCardData);
            playerDeck.Remove(tempCardData);
        }
        if (opponentDeck.Count > 0 && !isPlayerTurn)
        {
            tempCardData = GetRandomCard(opponentDeck);
            opponentCardHolder.AddCard(tempCardData);
            opponentDeck.Remove(tempCardData);
        }
    }

    public EntityData GetRandomCard(List<EntityData> deckToDraw)
    {
        int index = Random.Range(0, deckToDraw.Count);

        return deckToDraw[index];
    }

    public EntityData GetLastCard(List<EntityData> deckToDraw)
    {
        return deckToDraw[deckToDraw.Count - 1];
    }

    public void UpdateTurn(bool isPlayerTurn)
    {
        DrawCard(isPlayerTurn);

        if (isPlayerTurn)
        {
            playerCardHolder.cg.DOFade(1f, .1f);

            foreach (var card in playerCardHolder.cards)
            {
                card.CardRenderer.gr.enabled = true;
            }
        }
        else
        {
            playerCardHolder.cg.DOFade(.5f, .1f);

            foreach (var card in playerCardHolder.cards)
            {
                card.CardRenderer.gr.enabled = false;
            }
        }
    }
}
