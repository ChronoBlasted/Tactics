using BaseTemplate.Behaviours;
using UnityEngine;

public class MatchManager : MonoSingleton<MatchManager>
{
    public bool isPlayerTurn;

    public void Init()
    {
        ChooseWhosFirst();

        UpdateTurn();
    }

    public bool ChooseWhosFirst()
    {
        int randomValue = Random.Range(0, 2);

        isPlayerTurn = randomValue == 0;

        return isPlayerTurn;
    }

    void UpdateTurn()
    {
        if (isPlayerTurn)
        {

        }
        else
        {

        }
    }

    public void NextTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        GameEventSystem.Instance.feed[gameObject]?.Invoke();
    }

    public void PlayCard(GameObject cardToPlay)
    {

    }

    public void AddStatus(GameObject statusToAdd, GameObject cardToPlay)
    {

    }
}
