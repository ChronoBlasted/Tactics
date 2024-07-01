using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACard : ALife
{
    public List<Status> StatusList;
    public int BonusAttack;
    public int BonusMaxHealth;
    public CardRenderer CardRenderer;

    public void Init()
    {
        CardRenderer.Init(EntityData);

        GameEventSystem.Instance.AddEvent(EventType.ONSTARTTURN, OnStartTurn);
        GameEventSystem.Instance.AddEvent(EventType.ONENDTURN, OnEndTurn);
    }

    public void Attack()
    {

    }

    public void PlayCard()
    {
        Debug.Log("Card play");
    }

    public override bool TakeDamage(int amountDamage)
    {
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

    public void AddHealth()
    {

    }

    public void AddAttack()
    {

    }

    public void OnStartTurn(object[] actionData = null)
    {
    }

    public void Sacrifice()
    {

    }

    public void OnEndTurn(object[] actionData = null)
    {
        if (StatusList.Contains(Status.BURN)) TakeDamage(1);
    }

    public int GetAttack() => BonusAttack + EntityData.attack;

    public int GetMaxHealth() => BonusMaxHealth + EntityData.maxHealth;

}
