using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEntityData", menuName = "Entity Data", order = 51)]
public class EntityData : ScriptableObject
{
    public string entityName;
    public Sprite visual;
    public Element element;
    public string description;
    public int attack;
    public int maxHealth;
    public int level;
    public Status status;
}
