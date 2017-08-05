using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

    ItemDatabase database;

    //UI objects being managed
    public GameObject inventoryPanel;
    public GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    public int slotAmount;

    public List<Item> items = new List<Item>();
    public List<GameObject> slots = new List<GameObject>();

    void Start ()
    {
        database = GetComponent<ItemDatabase>();

        //create as many empty slots as desired
        for (int i = 0; i < slotAmount; i++)
        {
            GameObject newSlot = Instantiate(inventorySlot);

            //assign ID
            newSlot.GetComponent<InventorySlot>().slotID = i;

            //make child of Slot Panel for Grid Layout Group magic to happen
            newSlot.transform.SetParent(slotPanel.transform);

            //add to slot list
            slots.Add(newSlot);

            //add empty item to inventory to keep 1 to 1 ratio
            items.Add(new Item());

        }
    }

    void Update()
    {
        //PROTO: spawn random weapons
        if (Input.GetButtonDown("SpawnWeapon"))
        {
            AddItem((int)Mathf.Ceil(Random.Range(0, 3)));
            Debug.Log(gameObject.name + ": Spawned a random weapon for debug");
        }
    }

    public void AddItem(int id)
    {
        Item itemToAdd = database.FetchItemByID(id);

        //stack if item is stackable and already in inventory
        if (itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd))
        {
            //OPTIMISE: redundant, the present stack has already been found in the CheckIfItemIsInInventory() function call...
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == id)
                {
                    //Item is the only child of each slot, therefore GetChild(0) works
                    ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
                    data.amount++;
                    data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();

                    //get out of "for" loop if a stack has been found: don't add one item to each stack!
                    break;
                }
            }
        }
        else //new item not present in inventory
        { 
            for (int i = 0; i < items.Count; i++)
            {
                //if there is a free slot
                if (items[i].ID == -1)
                {
                    items[i] = itemToAdd;

                    //create visualization for item, position relative to parent slot
                    GameObject itemObject = Instantiate(inventoryItem);
                    itemObject.gameObject.GetComponent<ItemData>().item = itemToAdd;

                    //OPTIMISE: remember item original slot for Drag'n'Drop logic, a bit messy, could also cash ItemData into a variable
                    itemObject.gameObject.GetComponent<ItemData>().slotID = i;

                    itemObject.gameObject.GetComponent<ItemData>().amount = 1;

                    //positioning and hierarchy
                    itemObject.transform.SetParent(slots[i].transform);
                    itemObject.transform.localPosition = Vector2.zero;

                    itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite;
                    itemObject.name = itemToAdd.Title;

                    //don't add the item to every free slot: get out of the loop!
                    break;
                }
            }
        }
    }

    bool CheckIfItemIsInInventory(Item item) //this could return the slot number where the item was found, to simplify calculation
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == item.ID)
            {
                return true;
            }
        }

        return false;
    }
}
