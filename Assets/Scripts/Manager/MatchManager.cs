using BaseTemplate.Behaviours;
using UnityEngine;

public class MatchManager : MonoSingleton<MatchManager>
{
    public Board Board;
    [SerializeField] bool isPlayerTurn;

    int roundCount;

    public void Init()
    {
        GameEventSystem.Instance.AddEvent(EventType.ATTACK, ProcessAttack);
        GameEventSystem.Instance.AddEvent(EventType.PLAYCARD, PlayCard);
        GameEventSystem.Instance.AddEvent(EventType.DIE, DieCard);

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

        Board.UpdateTurn(isPlayerTurn);
        DeckManager.Instance.UpdateTurn(isPlayerTurn);

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
        SacrificePopup popup = UIManager.Instance.SacrificePopup;
        if (card.EntityData.level > 6)
        {
            popup.UpdateAmount(2);
            UIManager.Instance.AddPopup(popup);
        }
        else if (card.EntityData.level > 2)
        {
            popup.UpdateAmount(1);
            UIManager.Instance.AddPopup(popup);
        }
        else
        {
            Board.SpawnCard(isPlayerTurn, card);

            card.PlayCard();
        }
    }

    public void DieCard(object[] cardToPlay)
    {
        ACard card = (ACard)cardToPlay[0];

        card.Die();
    }

    public void AddStatus(Status statusToAdd, ACard cardToAffect)
    {
        cardToAffect.StatusList.Add(statusToAdd);
    }

    public void ProcessAttack(object[] cards)
    {
        ACard attacker = (ACard)cards[0];
        ACard defenser = (ACard)cards[1];

        if (attacker.StatusList.Contains(Status.STUN)) AddStatus(Status.STUN, defenser);
    }
}
