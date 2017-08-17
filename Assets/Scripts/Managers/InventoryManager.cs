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

    public List<ItemData> items = new List<ItemData>();
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
            items.Add(new ItemData());

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
        ItemData itemToAdd = database.FetchItemByID(id);

        //stack if item is stackable and already in inventory
        if (itemToAdd.Stackable && CheckIfItemIsInInventory(itemToAdd.ID))
        {
            //OPTIMISE: redundant, the present stack has already been found in the CheckIfItemIsInInventory() function call...
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ID == id)
                {
                    //Item is the only child of each slot, therefore GetChild(0) works
                    UIItem data = slots[i].transform.GetChild(0).GetComponent<UIItem>();
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
                    itemObject.gameObject.GetComponent<UIItem>().item = itemToAdd;

                    //OPTIMISE: remember item original slot for Drag'n'Drop logic, a bit messy, could also cash ItemData into a variable
                    itemObject.gameObject.GetComponent<UIItem>().slotID = i;

                    itemObject.gameObject.GetComponent<UIItem>().amount = 1;

                    //positioning and hierarchy
                    itemObject.transform.SetParent(slots[i].transform);
                    itemObject.transform.localPosition = Vector2.zero;

                    itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite;
					itemObject.name = itemToAdd.Title;

					if (itemToAdd.Type == ItemData.ItemType.FireArm) {
						GunBehaviour gunLogic = itemObject.AddComponent<GunBehaviour>();
						gunLogic.GunData = Resources.Load("scriptable_objects/guns/" + itemToAdd.Slug) as GunData;

						gunLogic.InitialChecks();
					}
					else if (itemToAdd.Type == ItemData.ItemType.Projectile)
					{
						ThrowBehaviour throwLogic = itemObject.AddComponent<ThrowBehaviour>();
						throwLogic.ProjectileData = Resources.Load("scriptable_objects/projectiles/" + itemToAdd.Slug) as ProjectileData;

						throwLogic.InitialChecks();
					}

					//don't add the item to every free slot: get out of the loop!
					break;
                }
            }
        }
    }

	public void RemoveItem(int id)
	{
		ItemData itemToRemove = database.FetchItemByID(id);

		//stack if item is stackable and already in inventory
		if (itemToRemove.Stackable && CheckIfItemIsInInventory(itemToRemove.ID))
		{
			//OPTIMISE: redundant, the present stack has already been found in the CheckIfItemIsInInventory() function call...
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i].ID == id)
				{
					//Item is the only child of each slot, therefore GetChild(0) works
					UIItem data = slots[i].transform.GetChild(0).GetComponent<UIItem>();
					data.amount--;

					if (data.amount == 0)
					{
						//delete item from slot
						items.Remove(items[i]);
						GameObject.Destroy(slots[i].transform.GetChild(0).gameObject);
					}

					data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();

					//get out of "for" loop if a stack has been found: don't add one item to each stack!
					break;
				}
			}
		}
		else if (!itemToRemove.Stackable && CheckIfItemIsInInventory(itemToRemove.ID))
		{
			//todo
		}
		else //new item not present in inventory
		{
			Debug.Log("ATTEMPTING TO REMOVE NON EXISTING ITEM IN INVENTORY // OUT OF AMMO");
		}
	}

	public bool CheckIfItemIsInInventory(int id) //this could return the slot number where the item was found, to simplify calculation
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].ID == id)
            {
                return true;
            }
        }

        return false;
    }

	public void ToggleItemEquip(UIItem itemToToggleEquip)
	{
		itemToToggleEquip.inUse = !itemToToggleEquip.inUse;

		//gross prototype code
		if (itemToToggleEquip.transform.parent.gameObject.GetComponent<Image>().color != Color.yellow)
			itemToToggleEquip.transform.parent.gameObject.GetComponent<Image>().color = Color.yellow;
		else
			itemToToggleEquip.transform.parent.gameObject.GetComponent<Image>().color = Color.white;
	}
}
