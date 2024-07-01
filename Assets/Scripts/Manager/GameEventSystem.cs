using BaseTemplate.Behaviours;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSystem : MonoSingleton<GameEventSystem>
{
    Dictionary<EventType, Action<object[]>> feed = new Dictionary<EventType, Action<object[]>>();

    public void Init()
    {

    }

    public void AddEvent(EventType eventType, Action<object[]> actionData)
    {
        if (feed.ContainsKey(eventType))
        {
            feed[eventType] += actionData;
        }
        else
        {
            feed.Add(eventType, actionData);
        }
    }

    public void RemoveEvent(EventType eventType, Action<object[]> actionData)
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

    public void Send(EventType eventType, object[] actionData)
    {
        if (feed.ContainsKey(eventType))
        {
            feed[eventType]?.Invoke(actionData);
        }
    }
}
