using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
    void Update()
    {

		
		if (Input.GetKeyDown(KeyCode.F5))
		{
			Debug.Break();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		/* Event-based inputs, not currently used */

		//if (Input.GetButtonDown("Inventory"))
  //      {
  //          EventManager.TriggerEvent("InventoryToggle");
  //          Debug.Log("INVENTORY EVENT");
  //      }

  //      if (Input.GetButtonDown("Pause"))
  //      {
  //          EventManager.TriggerEvent("PauseToggle");
  //          Debug.Log("PAUSE EVENT");
  //      }

  //      if (Input.GetButtonDown("Menu"))
  //      {
  //          EventManager.TriggerEvent("MenuToggle");
  //          Debug.Log("MENU EVENT");
  //      }
		
	}
}
