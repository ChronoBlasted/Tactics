using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALife : ABase
{
    public int Health;
    public int MaxHealth;
    public int BonusMaxHealth;

    public EntityData EntityData;

    public virtual bool TakeDamage(int amountDamage)
    {
        Health -= amountDamage;

        if (Health <= 0)
        {
            HealthDie();

            return true;
        }
        return false;
    }

    public virtual void HealthDie()
    {

    }
}
