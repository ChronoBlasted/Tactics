using BaseTemplate.Behaviours;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoSingleton<DeckManager>
{
    [SerializeField] int amountOfCardAtStart = 7;
    [SerializeField] List<EntityData> playerDeck = new List<EntityData>();
    [SerializeField] List<EntityData> opponentDeck = new List<EntityData>();

    [SerializeField] CardHolder playerCardHolder;
    [SerializeField] CardHolder opponentCardHolder;

    public void Init()
    {
        SetupHands();
    }

    public void SetupHands()
    {
        for (int i = 0; i < amountOfCardAtStart; i++)
        {
            playerCardHolder.AddCard(DrawCard(playerDeck));
        }

        for (int i = 0; i < amountOfCardAtStart; i++)
        {
            opponentCardHolder.AddCard(DrawCard(playerDeck));
        }
    }

    public EntityData DrawCard(List<EntityData> deckToDraw)
    {
        return deckToDraw[deckToDraw.Count - 1];
    }
}
