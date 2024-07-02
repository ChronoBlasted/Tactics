using System.Collections.Generic;
using System.Linq;
using BaseTemplate.Behaviours;
using Unity.VisualScripting;
using UnityEngine;

public class MatchManager : MonoSingleton<MatchManager>
{
    public Board Board;
    public APlayer Player, Opponent;

    public GameObject[] ListSlotPlayer, ListSlotEnemy;

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
        if (roundCount != 1)
        {
            Attack(isPlayerTurn);
            GameEventSystem.Instance.Send(EventType.ONENDTURN, null);
        }

        GameEventSystem.Instance.Send(EventType.NEWTURN, null);
        GameEventSystem.Instance.Send(EventType.ONSTARTTURN, null);

        Board.UpdateTurn(isPlayerTurn);
        DeckManager.Instance.UpdateTurn(isPlayerTurn);

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
        List<ACard> benchToSeek = card.isPlayerCard ? Board.playerCardBench : Board.opponentCardBench;

        if (card.EntityData.level > 6)
        {
            if (benchToSeek.Count < 2)
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
            if (benchToSeek.Count < 1)
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

        firstCard = false; // TODO a mettre ailleurs
    }

    public void CheckFamily(List<ACard> benchToSeek)
    {
        for (int i = 0; i < benchToSeek.Count; i++)
        {
            if (benchToSeek[i].EntityData.element == Element.FIRE) fire++;
            if (benchToSeek[i].EntityData.element == Element.GRASS) grass++;
            if (benchToSeek[i].EntityData.element == Element.WATER) water++;
        }

        AddBoostFamily(benchToSeek);
    }

    public void AddBoostFamily(List<ACard> benchToSeek)
    {
        if (fire > 2)
        {
            for (int i = 0; i < benchToSeek.Count; i++)
            {
                AddStatusOnImpact(Status.BURN, benchToSeek[i]);
            }
        }
        else if (water > 2)
        {
            for (int i = 0; i < benchToSeek.Count; i++)
            {
                AddStatusOnImpact(Status.OVERWHELM, benchToSeek[i]);
            }
        }
        else if (grass > 2)
        {
            for (int i = 0; i < benchToSeek.Count; i++)
            {
                AddStatus(Status.GROWTH, benchToSeek[i]);
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
        if (cardToAffect.StatusList.Contains(statusToAdd) == false)
        {
            cardToAffect.AddStatus(statusToAdd);
        }
    }

    public void AddStatusOnImpact(Status statusToAdd, ACard cardToAffect)
    {
        if (cardToAffect.StatusOnImpact.Contains(statusToAdd) == false)
        {
            cardToAffect.AddImpactStatus(statusToAdd);
        }
    }

    public void RemoveStatus(Status statusToRemove, ACard cardToAffect)
    {
        if (cardToAffect.StatusList.Contains(statusToRemove))
        {
            cardToAffect.RemoveStatus(statusToRemove);
        }
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

        if (attacker.StatusOnImpact.Contains(Status.OVERWHELM))
        {
            if (attacker.GetAttack() > defenser.Health)
            {
                playerDefend.TakeDamage(attacker.GetAttack() - defenser.Health);
            }
        }

        if (attacker.Attack(defenser))
        {
            defenser.Attack(attacker);
        }
        else
        {
            if (!attacker.StatusList.Contains(Status.STUN)) attacker.Attack(defenser);
            defenser.Attack(attacker);
            if (attacker.StatusOnImpact.Contains(Status.STUN)) AddStatus(Status.STUN, defenser);
        }

        if (attacker.StatusOnImpact.Contains(Status.STUN))
        {
            if (defenser.StatusList.Contains(Status.VIGILANT)) RemoveStatus(Status.VIGILANT, defenser);
            else AddStatus(Status.STUN, defenser);
        }

        if (attacker.StatusOnImpact.Contains(Status.BURN)) AddStatus(Status.BURN, defenser);
        if (attacker.StatusOnImpact.Contains(Status.CURSE)) AddStatus(Status.CURSE, defenser);
    }

    public void Attack(bool isPlayerTurn)
    {
        ACard attacker;
        ACard defender;
        if (!isPlayerTurn)
        {
            for (int i = 0; i < ListSlotPlayer.Count(); i++)
            {
                if (ListSlotPlayer[i].transform.childCount != 0)
                {
                    attacker = ListSlotPlayer[i].transform.GetChild(0).GetComponent<ACard>();

                    if (ListSlotEnemy[i].transform.childCount == 0)
                    {
                        defender = null;
                        var data = new object[] { attacker, defender, Opponent };
                        ProcessAttack(data);
                    }
                    else
                    {
                        defender = ListSlotEnemy[i].transform.GetChild(0).GetComponent<ACard>();
                        var data = new object[] { attacker, defender, Opponent };
                        ProcessAttack(data);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < ListSlotEnemy.Count(); i++)
            {
                if (ListSlotEnemy[i].transform.childCount != 0)
                {
                    attacker = ListSlotEnemy[i].transform.GetChild(0).GetComponent<ACard>();

                    if (ListSlotPlayer[i].transform.childCount == 0)
                    {
                        defender = null;
                        var data = new object[] { attacker, defender, Player };
                        ProcessAttack(data);
                    }
                    else
                    {
                        defender = ListSlotPlayer[i].transform.GetChild(0).GetComponent<ACard>();
                        var data = new object[] { attacker, defender, Player };
                        ProcessAttack(data);
                    }
                }
            }
        }
    }
}