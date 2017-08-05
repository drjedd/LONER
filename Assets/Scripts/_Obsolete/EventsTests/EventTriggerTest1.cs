using UnityEngine;
using System.Collections;

public class EventTriggerTest1 : MonoBehaviour {

	void Update () {
	    if (Input.GetKeyDown(KeyCode.Q))
        {
            EventManager.TriggerEvent("testEvent");
        }
	}
}
