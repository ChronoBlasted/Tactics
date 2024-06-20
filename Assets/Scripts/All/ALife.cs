using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALife : ABase
{
    public int Health;
    public EntityData EntityData;

    public bool TakeDamage()
    {
        return true;
    }
    
    public void die()
    {
        Destroy(gameObject);
    }
}
