using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardRenderer : MonoBehaviour
{
    EntityData data;

    [SerializeField] SpriteRenderer background;
    [SerializeField] SpriteRenderer visual;
    [SerializeField] TextMesh nameTxt;
    [SerializeField] TextMesh descTxt;
    [SerializeField] TextMesh attackTxt;
    [SerializeField] TextMesh heatlhTxt;

    public void Init(EntityData newData)
    {
        data = newData;

        visual.sprite = data.visual;

        nameTxt.text = data.name;
        descTxt.text = data.description;
        attackTxt.text = data.attack.ToString();
        heatlhTxt.text = data.maxHealth.ToString();
    }
}
