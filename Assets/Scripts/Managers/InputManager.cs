using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	

    //public PlayerPhysicsBehaviour playerPhysics;

    //void Start()
    //{
    //    //security checks
    //    if (playerPhysics == null)
    //    {
    //        GameObject player = GameObject.FindGameObjectWithTag("Player");

    //        if (player == null)
    //        {
    //            Debug.LogError(gameObject.name + ": Could not find a player in the scene. Make sure it has the tag \"Player\". Also make sure to add a PlayerPhysicsBehaviour component to it.");
    //        }
    //        else
    //        {
    //            playerPhysics = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPhysicsBehaviour>();

    //            if (playerPhysics == null)
    //            {
    //                Debug.LogError(gameObject.name + ": Could not find a PlayerPhysicsBehaviour component attached to the player game object (" + player.name + ")");
    //            }
    //        }
    //    }
    //}

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

		if (Input.GetButtonDown("Inventory"))
        {
            EventManager.TriggerEvent("InventoryToggle");
            //Debug.Log("INVENTORY EVENT");
        }

        if (Input.GetButtonDown("Pause"))
        {
            EventManager.TriggerEvent("PauseToggle");
            Debug.Log("PAUSE EVENT");
        }

        if (Input.GetButtonDown("Menu"))
        {
            EventManager.TriggerEvent("MenuToggle");
            Debug.Log("MENU EVENT");
        }
		
	}
}
