using System.Linq;
using BaseTemplate.Behaviours;
using Unity.VisualScripting;
using UnityEngine;

public class MatchManager : MonoSingleton<MatchManager>
{
    public Board Board;
    public bool isPlayerTurn;

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
            if (Board.playerCardBench.Count < 2)
            {
                card.CardRenderer.ResetCardInHand();
                UIManager.Instance.DoFloatingText("You need atleast 2 card to play this card", Color.red);
            }
            else
            {
                popup.UpdateData(2, card);
                UIManager.Instance.AddPopup(popup);
            }
        }
        else if (card.EntityData.level > 2)
        {
            if (Board.playerCardBench.Count < 1)
            {
                card.CardRenderer.ResetCardInHand();
                UIManager.Instance.DoFloatingText("You need atleast 1 card to play this card", Color.red);
            }
            else
            {
                popup.UpdateData(1, card);
                UIManager.Instance.AddPopup(popup);
            }
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

    public void RemoveStatus(ACard cardToEffect, Status statusToRemove)
    {
        cardToEffect.StatusList.Remove(statusToRemove);
    }

    public void ProcessAttack(object[] data)
    {
        ACard attacker = (ACard)data[0];
        ACard defenser = (ACard)data[1];
        APlayer playerDefend = (APlayer)data[2];

        if (defenser == null)
        {
            playerDefend.TakeDamage(attacker.GetAttack());
            return;
        }
        if (attacker.StatusList.Contains(Status.OVERWHELM))
        {
            if (attacker.GetAttack() > defenser.Health)
            {
                playerDefend.TakeDamage(attacker.GetAttack() - defenser.Health);
            }
        }
        if (!attacker.StatusList.Contains(Status.STUN)) attacker.Attack(defenser);
        defenser.Attack(attacker);
        if (attacker.StatusList.Contains(Status.BURN)) AddStatus(Status.BURN, defenser);
        if (attacker.StatusList.Contains(Status.CURSE)) AddStatus(Status.CURSE, defenser);
        if (defenser.StatusList.Contains(Status.VIGILANT))
        {
            RemoveStatus(defenser, Status.VIGILANT);
            return;
        }
        if (attacker.StatusList.Contains(Status.STUN)) AddStatus(Status.STUN, defenser);
    }
}
