using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACard : ALife
{
    public List<Status> StatusList;
    public int BonusAttack;
    public CardRenderer CardRenderer;

    public void Init()
    {
        CardRenderer.Init(EntityData);

        GameEventSystem.Instance.AddEvent(EventType.ONSTARTTURN, OnStartTurn);
        GameEventSystem.Instance.AddEvent(EventType.ONENDTURN, OnEndTurn);

        Health = GetMaxHealth();
    }

    public void Attack(ALife enemyLife)
    {
        if (StatusList.Contains(Status.STUN)) return;
        enemyLife.TakeDamage(GetAttack());

    }

    public void PlayCard()
    {
        if (MatchManager.Instance.firstCard)
        {
            if (StatusList.Contains(Status.DAYBREAK))
            {
                AddAttack(2);
            }
        }

        if (!MatchManager.Instance.firstCard)
        {
            if (StatusList.Contains(Status.NIGHTFALL))
            {
                AddHealth(1);
            }
        }

        if (DeckManager.Instance.playerCardHolder.cards.Contains(this)) DeckManager.Instance.playerCardHolder.cards.Remove(this);
        if (DeckManager.Instance.opponentCardHolder.cards.Contains(this)) DeckManager.Instance.opponentCardHolder.cards.Remove(this);
    }

    public override bool TakeDamage(int amountDamage)
    {
        if (StatusList.Contains(Status.ROBUST)) amountDamage--;
        return base.TakeDamage(amountDamage);
    }

    public override void HealthDie()
    {
        base.HealthDie();

        var data = new object[] { this };

        GameEventSystem.Instance.Send(EventType.DIE, data);
    }

    public void Die()
    {

        Debug.Log("Die");
    }

    public void AddHealth(int healthAdd)
    {
        BonusMaxHealth += healthAdd;
        Health += healthAdd;
    }

    public void AddAttack(int attackAdd)
    {
        BonusAttack += attackAdd;

        if (BonusAttack < 0)
        {
            BonusAttack = 0;
        }
    }

    public void OnStartTurn(object[] actionData = null)
    {
        if (StatusList.Contains(Status.REGENERATION)) Health = GetMaxHealth();
    }

    public void Sacrifice()
    {
        //Sacrifice

        if (MatchManager.Instance.Board.playerCardBench.Contains(this)) MatchManager.Instance.Board.playerCardBench.Remove(this);
        if (MatchManager.Instance.Board.opponentCardBench.Contains(this)) MatchManager.Instance.Board.opponentCardBench.Remove(this);

        CardRenderer.MoveUpTween.Kill();

        Destroy(gameObject);
    }

    public void OnEndTurn(object[] actionData = null)
    {
        if (StatusList.Contains(Status.BURN)) TakeDamage(1);
        if (StatusList.Contains(Status.CURSE)) AddAttack(-1);
        if (StatusList.Contains(Status.GROWTH)) AddHealth(1);
    }

    public int GetAttack() => BonusAttack + EntityData.attack;

    public int GetMaxHealth() => BonusMaxHealth + EntityData.maxHealth;

}
