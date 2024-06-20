using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACard : ALife
{
    public List<Status> StatusList;
    public int BonusAttack;
    public int BonusMaxHealth;
    
    public void Attack()
    {
        
    }

    public void PlayCard()
    {
        
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
