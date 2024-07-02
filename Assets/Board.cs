using DG.Tweening;
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
        List<ACard> cardBench = isPlayerCard ? playerCardBench : opponentCardBench;

        cardBench.Add(cardToPlay);

        // Trouver le premier slot libre
        for (int i = 0; i < sideToSpawn.childCount; i++)
        {
            Transform slot = sideToSpawn.GetChild(i);
            if (slot.childCount == 0)
            {
                cardToPlay.CardRenderer.transform.SetParent(UIManager.Instance.MainCanvas.transform);

                cardToPlay.transform.SetParent(slot);
                cardToPlay.transform.localPosition = Vector3.zero;

                cardToPlay.CardRenderer.transform.SetParent(cardToPlay.transform);
                cardToPlay.CardRenderer.transform.localPosition = Vector3.zero;
                break;
            }
        }
    }

    public void UpdateTurn(bool isPlayerTurn)
    {
        whosTurn.sprite = isPlayerTurn ? playerTurnSprite : opponentTurnSprite;
    }
}
