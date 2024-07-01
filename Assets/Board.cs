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
        Transform sideToSpawn = isPlayerCard ? playerSideBench : opponentSideBench;

        if (isPlayerCard) playerCardBench.Add(cardToPlay);
        else opponentCardBench.Add(cardToPlay);

        cardToPlay.CardRenderer.transform.SetParent(UIManager.Instance.MainCanvas.transform);

        cardToPlay.transform.SetParent(sideToSpawn);

        cardToPlay.transform.localPosition = Vector3.zero;

        yield return new WaitForEndOfFrame();

        cardToPlay.CardRenderer.transform.SetParent(cardToPlay.transform);

        cardToPlay.CardRenderer.transform.DOLocalMove(Vector3.zero, .2f);
    }

    public void UpdateTurn(bool isPlayerTurn)
    {
        whosTurn.sprite = isPlayerTurn ? playerTurnSprite : opponentTurnSprite;
    }
}
