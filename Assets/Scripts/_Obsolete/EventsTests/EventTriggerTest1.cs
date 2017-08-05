using UnityEngine;
using System.Collections;

public class EventTriggerTest1 : MonoBehaviour {

	void Update () {
	    if (Input.GetKeyDown(KeyCode.B))
        {
            EventManager.TriggerEvent("testEvent");
        }

		if (Input.GetKeyDown(KeyCode.N))
		{
			EventManager.TriggerEvent("testEvent2");
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			EventManager.TriggerEvent("testEvent3");
		}
	}
}
