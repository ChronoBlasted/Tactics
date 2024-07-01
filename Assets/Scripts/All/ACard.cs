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
    }

    public void Attack()
    {

    }

    public void PlayCard()
    {
        var data = new object[] { this };

        GameEventSystem.Instance.Send(EventType.PLAYCARD, data);
    }

    public void PlaceOnAttack()
    {

    }

    public void AddHealth()
    {

    }

    public void AddAttack()
    {

    }

    public void OnStartTurn()
    {

    }

    public void Sacrifice()
    {

    }

    public void OnEndTurn()
    {

    }

    public int GetAttack() => BonusAttack + EntityData.attack;

    public int GetMaxHealth() => BonusMaxHealth + EntityData.maxHealth;

}
