using BaseTemplate.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoSingleton<ColorManager>
{
    public Color red;
    public Color green;
    public Color blue;

    [SerializeField] Sprite normalBG;
    [SerializeField] Sprite fireBG;
    [SerializeField] Sprite waterBG;
    [SerializeField] Sprite grassBG;

    public Sprite GetBackgroundByElement(Element element)
    {
        switch (element)
        {
            case Element.NORMAL:
                return normalBG;
            case Element.FIRE:
                return fireBG;
            case Element.WATER:
                return waterBG;
            case Element.GRASS:
                return grassBG;
        }

        return normalBG;
    }
}
