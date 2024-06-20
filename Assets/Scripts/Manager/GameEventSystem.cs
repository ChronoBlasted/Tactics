using BaseTemplate.Behaviours;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoSingleton<GameEventSystem>
{
    public Dictionary<GameObject, Action> feed;

    public void AddEvent(GameObject eventType, Action actionData)
    {
        feed[eventType] += actionData;

        feed[eventType]?.Invoke();
    }

    public void RemoveEvent(GameObject eventType, Action actionData)
    {
        if (feed.ContainsKey(eventType))
        {
            feed[eventType] -= actionData;
        }
        else
        {
            Debug.Log("Feed ne possède pas cette event type");
        }
    }
}
