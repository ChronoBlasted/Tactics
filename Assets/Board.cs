using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] Transform playerSideBench;
    [SerializeField] Transform opponentSideBench;

    public List<ACard> playerCardBench;
    public List<ACard> opponentCardBench;

    [SerializeField] Image whosTurn;
    [SerializeField] Sprite playerTurnSprite, opponentTurnSprite;

    public void SpawnCard(bool isPlayerCard, ACard cardToPlay)
    {
        Transform sideToSpawn = isPlayerCard ? playerSideBench : opponentSideBench;

        if (isPlayerCard) playerCardBench.Add(cardToPlay);
        else opponentCardBench.Add(cardToPlay);

        cardToPlay.transform.SetParent(sideToSpawn);

        cardToPlay.transform.localPosition = Vector3.zero;
        cardToPlay.CardRenderer.transform.localPosition = Vector3.zero;
    }

    public void UpdateTurn(bool isPlayerTurn)
    {
        whosTurn.sprite = isPlayerTurn ? playerTurnSprite : opponentTurnSprite;
    }
}
