using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : View
{
    [SerializeField] Button _nextTurnButton;

    bool isPlayerTurn;

    public void HandleOnNextTurn()
    {
        // Ping le GES
    }

    public void UpdateTurn()
    {
        _nextTurnButton.interactable = isPlayerTurn;

        isPlayerTurn = !isPlayerTurn;

    }
}
