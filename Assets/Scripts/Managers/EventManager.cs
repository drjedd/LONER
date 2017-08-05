using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//design and code from Unity Live Training April 13th, 2015: https://www.youtube.com/watch?v=0AqG1fDhPT8

/*
 * TODO: OPTIMIZE
 * 1) static class not derived from MonoBehaviour (not linked to gameobject)
 * 2) security: prevent classes from deleting events, etc.
 * Advantage of MonoBehaviour is: cleanup when changing scene, object is destroyed therefore no need to manage memory etc.
 */

public class EventManager : MonoBehaviour {

    //primary core of this event system
    private Dictionary<string, UnityEvent> eventDictionary;

    private static EventManager eventManager;
    public static EventManager instance
    {
        get
        {
            //safety checks
            if (!eventManager)
            {
                eventManager = FindObjectOfType (typeof (EventManager)) as EventManager;

                if (!eventManager)
                {
                    //It makes no sense to keep going if the objectmanager is not present. It is a crucial component for the game.
                    Debug.LogError("Event Manager (static): There needs to be one active EventManager script on a GameObject in your scene");
                }
                else
                {
                    eventManager.Initialize();
                }
            }

            return eventManager;
        }
    }

    void Initialize()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent>();
        }
    }
    
    public static void StartListening (string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;

        //if already in the dictionary, add to it
        if (instance.eventDictionary.TryGetValue(eventName,out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else //if not, create a new value
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening (string eventName, UnityAction listener)
    {
        //security: if Dictionary has been removed, return straight away
        if (eventManager == null)
        {
            Debug.LogError("EventManager: Dictionary cannot be found and has probably been removed/destroyed. (Order of clean up or other issue)");
            return;
        }

        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent (string eventName)
    {
        UnityEvent thisEvent = null;

        if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Z))
		{
			//List<string> eventNames = instance.eventDictionary.Keys.ToList();
			//List<UnityEvent> eventsList = instance.eventDictionary.Values.ToList();

			//Debug.Log(eventNames + " - " + eventsList);

			if (instance.eventDictionary.Count != 0)
			{
				foreach (string key in instance.eventDictionary.Keys)
				{
					UnityEvent uEvent = null;
					instance.eventDictionary.TryGetValue(key, out uEvent);
					Debug.Log(key + " - " + uEvent.ToString());
				}
			}
			else
			{
				Debug.Log("NOTHING IN EVENT DICTIONARY");
			}

		}
	}
}
