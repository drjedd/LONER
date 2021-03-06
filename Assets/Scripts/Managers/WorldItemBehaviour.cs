﻿using UnityEngine;
using System.Collections;

public class WorldItemBehaviour : MonoBehaviour {

    //IMPLEMENT: glowing items for rarity, or to help with locating quest items?
    public bool glowing;
    public int itemID;
	[Range (1, 100)]public int itemQuantity = 1;

    private Collider2D attachedCollider;
    private Rigidbody2D attachedRigidbody;

    private InventoryManager inventoryManager;

    void Start()
    {
        //security checks: look for inventory manager in scene, make sure proper component is attached - ASAP: use Singleton manager
        GameObject inventoryManagerObject = GameObject.FindGameObjectWithTag("InventoryManager");

        if (inventoryManagerObject == null)
        {
            Debug.LogError(gameObject.name + ": Can't find the Inventory Manager game object. It is necessary for item pick-up behaviour.");
        }
        else
        {
            inventoryManager = inventoryManagerObject.GetComponent<InventoryManager>();

            if (inventoryManager == null)
            {
                Debug.LogError(gameObject.name + ": Can't find the InventoryManager component attached to the Inventory Manager game object. It is necessary for item pick-up behaviour.");
            }
        }

        //check for other required components on Game Object:
        attachedRigidbody = GetComponent<Rigidbody2D>();

        if (attachedRigidbody == null)
        {
            Debug.Log(gameObject.name + ": No Rigidbody2D component attached, therefore collider is static which is not ideal for Unity if we want to move items around (see Unity colliders manual)");
        }

        attachedCollider = GetComponent<Collider2D>();

        if (attachedCollider == null)
        {
            Debug.LogError(gameObject.name + ": No Collider2D component attached, required for some functionality (e.g. pick-up item) to work. (Collision-based)");
        }
    }
	
    void OnTriggerEnter2D (Collider2D collider)
	{

		if (collider.gameObject.tag == "Player") {
			bool pickedUpAlright = false; //the shittiest variable name so far SeemsGood

			for (int i = 0; i < itemQuantity; i++) {
				pickedUpAlright = inventoryManager.AddItem (itemID); //pick up item, shitty prototype code
			}

			//FIXME: object gets destroyed even if inventory is full
			if (pickedUpAlright) {
				DestroyObject (gameObject);
				Debug.Log (gameObject.name + " was picked up by " + collider.gameObject.name);
			}
		}
    }
}
