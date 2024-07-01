using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SacrificePopup : Popup
{
    [SerializeField] TMP_Text sacrificeTxt;
    [SerializeField] int sortingLayerUp = 5, sortingLayerDown = 1;

    int _amountOfSacrifice;

    public override void Init()
    {
        base.Init();
    }

    public void UpdateAmount(int amountOfSacrifice)
    {
        _amountOfSacrifice = amountOfSacrifice;

        sacrificeTxt.text = "Choose " + _amountOfSacrifice + " card(s) to sacrifice !";
    }

    public override void OpenPopup()
    {
        base.OpenPopup();

        var allCard = MatchManager.Instance.Board.playerCardBench;

        foreach (var card in allCard)
        {
            card.CardRenderer.canvas.sortingOrder = sortingLayerUp;
        }
    }

    public override void ClosePopup()
    {
        base.ClosePopup();

        var allCard = MatchManager.Instance.Board.playerCardBench;

        foreach (var card in allCard)
        {
            card.CardRenderer.canvas.sortingOrder = sortingLayerDown;
        }
    }
}
