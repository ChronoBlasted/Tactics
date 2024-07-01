using BaseTemplate.Behaviours;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoSingleton<DeckManager>
{
    [SerializeField] int amountOfCardAtStart = 7;
    [SerializeField] int amountOfCardPerDeck = 20;
    [SerializeField] List<EntityData> allCard = new List<EntityData>();

    [SerializeField] List<EntityData> playerDeck = new List<EntityData>();
    [SerializeField] List<EntityData> opponentDeck = new List<EntityData>();

    [SerializeField] CardHolder playerCardHolder;
    [SerializeField] CardHolder opponentCardHolder;

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

    public void SetupHands()
    {
        for (int i = 0; i < amountOfCardAtStart; i++)
        {
            tempCardData = GetRandomCard(playerDeck);

            playerCardHolder.AddCard(tempCardData);

            playerDeck.Remove(tempCardData);
        }

        for (int i = 0; i < amountOfCardAtStart; i++)
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
        if (isPlayerTurn)
        {
            playerCardHolder.cg.alpha = 1f;
        }
        else
        {
            playerCardHolder.cg.alpha = .5f;

        }
    }
}
