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

    public IEnumerator SpawnCard(bool isPlayerCard, ACard cardToPlay)
    {
        yield return new WaitForEndOfFrame();

        Transform sideToSpawn = isPlayerCard ? playerSideBench : opponentSideBench;
        List<ACard> cardBench = isPlayerCard ? playerCardBench : opponentCardBench;

        if (cardBench.Count >= 5)
        {
            UIManager.Instance.DoFloatingText("You already have 5 card on the bench", Color.red);
            cardToPlay.CardRenderer.ResetCardInHand();
            yield break;

        }

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

        MatchManager.Instance.CheckFamily(cardBench);
    }

    public void DoSpawnCard(bool isPlayerCard, ACard cardToPlay)
    {
        StartCoroutine(MatchManager.Instance.Board.SpawnCard(isPlayerCard, cardToPlay));
    }

    public void UpdateTurn(bool isPlayerTurn)
    {
        if (isPlayerTurn) transform.DORotate(new Vector3(0, 0, 0), .2f).SetEase(Ease.OutSine);
        else transform.DORotate(new Vector3(0, 0, -180), .2f).SetEase(Ease.OutSine);
    }
}
