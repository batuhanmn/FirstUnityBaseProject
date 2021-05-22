
using UnityEngine;
using System.Collections.Generic;
using System;

public static class EventSystem
{
    private static Dictionary<string, Dictionary<string,Action<object[]>>> eventDictionary = new Dictionary<string, Dictionary<string,Action<object[]>>>();
    // EventSystem.StartListening("OnLevelSuccess", "LevelUI", UpdateLevelUI)
    public static void StartListening(string eventName, string caller, Action<object[]> listener)
    {
        Dictionary<string,Action<object[]>> events;
        if (eventDictionary.TryGetValue(eventName, out events))
        {
            events.Add(caller,listener);
        }
        else
        {
            eventDictionary.Add(eventName, new Dictionary<string,Action<object[]>>() { {caller,listener } });
        }
    }
    // EventSystem.StopListening("OnLevelSuccess", "LevelUI")
    public static void StopListening(string eventName, string caller)
    {
        Dictionary<string, Action<object[]>> events;
        if (eventDictionary.TryGetValue(eventName, out events))
        {
            events.Remove(caller);
        }
    }
    //EventSystem.TriggerEvent("OnLevelSuccess", "1", 2, "LevelName", new List<int>(){1,3})
    public static void TriggerEvent(string eventName,params object[] list)
    {
        Dictionary<string ,Action<object[]>> events;
        if (eventDictionary.TryGetValue(eventName, out events))
        {
            foreach (var item in events)
            {
                try{

                    item.Value(list);
                }
                catch(Exception e)
                {
                    Debug.Log("Exception at Event Name: "+ eventName +" on: "+item.Key+" error: "+ e);
                }
            }
        }
    }
}