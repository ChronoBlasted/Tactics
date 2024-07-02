using System.Linq;
using BaseTemplate.Behaviours;
using Unity.VisualScripting;
using UnityEngine;

public class MatchManager : MonoSingleton<MatchManager>
{
    public Board Board;
    public APlayer Player, Opponent;
    
    public bool isPlayerTurn;
    public bool firstCard;

    int fire, grass, water;

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
        Attack(isPlayerTurn);
        firstCard = true;
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
            Board.DoSpawnCard(isPlayerTurn, card);

            card.PlayCard();
        }

        firstCard = false;
        CheckFamily();
    }

    public void CheckFamily()
    {
        for (int i = 0; i < Board.playerCardBench.Count; i++)
        {
            if (Board.playerCardBench[i].EntityData.element == Element.FIRE) fire++;
            if (Board.playerCardBench[i].EntityData.element == Element.GRASS) grass++;
            if (Board.playerCardBench[i].EntityData.element == Element.WATER) water++;
        }
        AddBoostFamily();
    }

    public void AddBoostFamily()
    {
        if (fire > 2)
        {
            for (int i = 0; i < Board.opponentCardBench.Count; i++)
            {
                AddStatus(Status.BURN, Board.opponentCardBench[i]);
            }


        }
        else if (water > 2)
        {
            for (int i = 0; i < Board.opponentCardBench.Count; i++)
            {
                AddStatus(Status.OVERWHELM, Board.playerCardBench[i]);
            }


        }
        else if (grass > 2)
        {
            for (int i = 0; i < Board.opponentCardBench.Count; i++)
            {
                AddStatus(Status.GROWTH, Board.playerCardBench[i]);
            }
        }
        water = 0;
        grass = 0;
        fire = 0;
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

    public void Attack(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            for (int i = 0; i < Board.playerCardBench.Count(); i++)
            {
                if (Board.playerCardBench[i] == null)
                {
                    return;
                }
                var data = new object[] { Board.playerCardBench[i],Board.opponentCardBench[i], Opponent };
                ProcessAttack(data);
            }
        }
        else
        {
            for (int i = 0; i < Board.opponentCardBench.Count(); i++)
            {
                if (Board.opponentCardBench[i] == null)
                {
                    return;
                }
                var data = new object[] { Board.opponentCardBench[i],Board.playerCardBench[i], Player };
                ProcessAttack(data);
            }
        }

    }
}
