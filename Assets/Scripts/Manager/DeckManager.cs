using BaseTemplate.Behaviours;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoSingleton<DeckManager>
{
    [SerializeField] int amountOfCardAtStart = 7;
    [SerializeField] Queue<GameObject> playerDeck = new Queue<GameObject>();
    [SerializeField] Queue<GameObject> opponentDeck = new Queue<GameObject>();


    [SerializeField] List<GameObject> playerHand = new List<GameObject>();
    [SerializeField] List<GameObject> opponentHand = new List<GameObject>();

    public void Init()
    {
        SetupHands();
    }

    public void SetupHands()
    {
        for (int i = 0; i < amountOfCardAtStart; i++)
        {
            playerHand.Add(DrawCard(playerDeck));
        }

        for (int i = 0; i < amountOfCardAtStart; i++)
        {
            opponentHand.Add(DrawCard(opponentDeck));
        }
    }

    public GameObject DrawCard(Queue<GameObject> deckToDraw)
    {
        return deckToDraw.Dequeue();
    }
}
