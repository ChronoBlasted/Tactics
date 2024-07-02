using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACard : ALife
{
    List<Status> statusList = new List<Status>();
    List<Status> statusOnImpact = new List<Status>();

    public int BonusAttack;
    public CardRenderer CardRenderer;
    public bool isPlayerCard;

    public List<Status> StatusList { get => statusList; }
    public List<Status> StatusOnImpact { get => statusOnImpact; }

    public void Init(bool isPlayerCard)
    {

        this.isPlayerCard = isPlayerCard;

        CardRenderer.Init(EntityData);

        AddStatus(EntityData.status);
        AddImpactStatus(EntityData.StatusOnImpact);

        Health = GetMaxHealth();

        CardRenderer.UpdateHealth(Health);
        CardRenderer.UpdateDamage(GetAttack());
    }

    public bool Attack(ALife enemyLife)
    {
        if (statusList.Contains(Status.STUN))
        {
            RemoveStatus(Status.STUN);
            AddStatus(Status.VIGILANT);
            return false;
        }
        enemyLife.TakeDamage(GetAttack());

        return true;
    }

    public void PlayCard()
    {
        GameEventSystem.Instance.AddEvent(EventType.ONSTARTTURN, OnStartTurn);
        GameEventSystem.Instance.AddEvent(EventType.ONENDTURN, OnEndTurn);

        if (MatchManager.Instance.firstCard)
        {
            if (statusList.Contains(Status.DAYBREAK))
            {
                AddAttack(2);
            }
        }

        if (!MatchManager.Instance.firstCard)
        {
            if (statusList.Contains(Status.NIGHTFALL))
            {
                AddHealth(1);
            }
        }

        if (DeckManager.Instance.playerCardHolder.cards.Contains(this)) DeckManager.Instance.playerCardHolder.cards.Remove(this);
        if (DeckManager.Instance.opponentCardHolder.cards.Contains(this)) DeckManager.Instance.opponentCardHolder.cards.Remove(this);
    }

    public override bool TakeDamage(int amountDamage)
    {
        if (statusList.Contains(Status.ROBUST)) amountDamage--;

        var isDead = base.TakeDamage(amountDamage);

        CardRenderer.UpdateHealth(Health);

        return isDead;
    }

    public override void HealthDie()
    {
        base.HealthDie();

        var data = new object[] { this };

        GameEventSystem.Instance.Send(EventType.DIE, data);
    }

    public void Die()
    {
        if (MatchManager.Instance.Board.playerCardBench.Contains(this)) MatchManager.Instance.Board.playerCardBench.Remove(this);
        if (MatchManager.Instance.Board.opponentCardBench.Contains(this)) MatchManager.Instance.Board.opponentCardBench.Remove(this);

        CardRenderer.MoveUpTween.Kill();

        Destroy(gameObject);
    }

    public void AddHealth(int healthAdd)
    {
        BonusMaxHealth += healthAdd;
        Health += healthAdd;

        CardRenderer.UpdateHealth(Health);
    }

    public void AddAttack(int attackAdd)
    {
        BonusAttack += attackAdd;

        CardRenderer.UpdateDamage(GetAttack());
    }

    public void OnStartTurn(object[] actionData = null)
    {
        if (statusList.Contains(Status.REGENERATION))
        {
            Health = GetMaxHealth();

            CardRenderer.UpdateHealth(Health);
        }
    }

    public void Sacrifice()
    {
        if (MatchManager.Instance.Board.playerCardBench.Contains(this)) MatchManager.Instance.Board.playerCardBench.Remove(this);
        if (MatchManager.Instance.Board.opponentCardBench.Contains(this)) MatchManager.Instance.Board.opponentCardBench.Remove(this);

        CardRenderer.MoveUpTween.Kill();

        Destroy(gameObject);
    }

    public void OnEndTurn(object[] actionData = null)
    {
        if (statusList.Contains(Status.BURN)) TakeDamage(1);
        if (statusList.Contains(Status.CURSE)) AddAttack(-1);
        if (statusList.Contains(Status.GROWTH)) AddHealth(1);
    }

    public void AddStatus(Status statusToAdd)
    {
        if (statusToAdd == Status.NONE) return;

        statusList.Add(statusToAdd);

        CardRenderer.UpdateDesc();
    }
    public void RemoveStatus(Status statusToRemove)
    {
        statusList.Remove(statusToRemove);

        CardRenderer.UpdateDesc();
    }

    public void AddImpactStatus(Status statusToAdd)
    {
        if (statusToAdd == Status.NONE) return;

        statusOnImpact.Add(statusToAdd);

        CardRenderer.UpdateDesc();
    }
    public void RemoveImpactStatus(Status statusToAdd)
    {
        statusOnImpact.Remove(statusToAdd);

        CardRenderer.UpdateDesc();
    }

    public int GetAttack() => Math.Clamp(BonusAttack + EntityData.attack, 0, 999);

    public int GetMaxHealth() => BonusMaxHealth + EntityData.maxHealth;

}
