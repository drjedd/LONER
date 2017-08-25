using UnityEngine;
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
        UIItem droppedItem = eventData.pointerDrag.GetComponent<UIItem>();

        //if there is no item in desired drop slot
        if (inventory.inventoryItems[slotID].ID == -1)
        {
            //clear the slot the item is dragged from, BEFORE assigning new slot
            inventory.inventoryItems[droppedItem.slotID] = new ItemData();
            inventory.inventoryItems[slotID] = droppedItem.item;

            droppedItem.slotID = slotID;
        }
        else if (droppedItem.slotID != slotID) //fix for child not found error when dropping in the same slot
        {
            Transform swappedItem = this.transform.GetChild(0);

            //OPTIMISE: add a setter accessor to call a function that moves the gameObject automatically
            swappedItem.GetComponent<UIItem>().slotID = droppedItem.slotID;
            swappedItem.transform.SetParent(inventory.inventorySlots[droppedItem.slotID].transform);
            swappedItem.transform.position = inventory.inventorySlots[droppedItem.slotID].transform.position;

            droppedItem.slotID = slotID;
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;

            inventory.inventoryItems[droppedItem.slotID] = swappedItem.GetComponent<UIItem>().item;
            inventory.inventoryItems[slotID] = droppedItem.item;
        }
    }
}
