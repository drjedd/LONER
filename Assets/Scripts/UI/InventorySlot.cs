﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public int slotID;
    private InventoryManager inventory;

    void Start ()
    {
        inventory = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

        //if there is no item in desired drop slot
        if (inventory.items[slotID].ID == -1)
        {
            //clear the slot the item is dragged from, BEFORE assigning new slot
            inventory.items[droppedItem.slotID] = new Item();
            inventory.items[slotID] = droppedItem.item;

            droppedItem.slotID = slotID;
        }
        else if (droppedItem.slotID != slotID) //fix for child not found error when dropping in the same slot
        {
            Transform swappedItem = this.transform.GetChild(0);

            //OPTIMISE: add a setter accessor to call a function that moves the gameObject automatically
            swappedItem.GetComponent<ItemData>().slotID = droppedItem.slotID;
            swappedItem.transform.SetParent(inventory.slots[droppedItem.slotID].transform);
            swappedItem.transform.position = inventory.slots[droppedItem.slotID].transform.position;

            droppedItem.slotID = slotID;
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;

            inventory.items[droppedItem.slotID] = swappedItem.GetComponent<ItemData>().item;
            inventory.items[slotID] = droppedItem.item;
        }
    }
}
