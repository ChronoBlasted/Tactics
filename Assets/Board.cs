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

    public IEnumerator SpawnCard(bool isPlayerCard, ACard cardToPlay)
    {
        yield return new WaitForEndOfFrame();

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

                yield return new WaitForEndOfFrame();

                cardToPlay.CardRenderer.transform.SetParent(cardToPlay.transform);
                cardToPlay.CardRenderer.transform.DOLocalMove(Vector3.zero, .2f);
                break;
            }
        }
    }

    public void DoSpawnCard(bool isPlayerCard, ACard cardToPlay)
    {
        StartCoroutine(MatchManager.Instance.Board.SpawnCard(isPlayerCard, cardToPlay));
    }

    public void UpdateTurn(bool isPlayerTurn)
    {
        whosTurn.sprite = isPlayerTurn ? playerTurnSprite : opponentTurnSprite;
    }
}
