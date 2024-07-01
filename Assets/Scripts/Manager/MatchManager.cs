using BaseTemplate.Behaviours;
using UnityEngine;

public class MatchManager : MonoSingleton<MatchManager>
{
    [SerializeField] Board board;
    [SerializeField] bool isPlayerTurn;

    int roundCount;

    public void Init()
    {
        GameEventSystem.Instance.AddEvent(EventType.PLAYCARD, PlayCard);

        ChooseWhosFirst();
        UpdateTurn();
    }

    public bool ChooseWhosFirst()
    {
        int randomValue = Random.Range(0, 2);

        isPlayerTurn = randomValue == 0;

        return isPlayerTurn;
    }

    void UpdateTurn()
    {
        if (roundCount != 0) GameEventSystem.Instance.Send(EventType.ONENDTURN, null);
        GameEventSystem.Instance.Send(EventType.NEWTURN, null);
        GameEventSystem.Instance.Send(EventType.ONSTARTTURN, null);

        board.UpdateTurn(isPlayerTurn);

        roundCount++;
    }

    public void HandleNextTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        UpdateTurn();
    }

    public void PlayCard(object[] cardToPlay)
    {
        ACard card = (ACard)cardToPlay[0];

        if (card.EntityData.level > 6)
        {
            //Open sacrifice view
        }
        else if (card.EntityData.level > 2)
        {
            //Open sacrifice view
        }
        else
        {
            board.SpawnCard(isPlayerTurn, card);

            card.PlayCard();
        }
    }

    public void AddStatus(Status statusToAdd, ACard cardToAffect)
    {
        cardToAffect.StatusList.Add(statusToAdd);
    }
}
