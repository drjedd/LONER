using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

    ItemDatabase database;

	//super gross reference with NO CHECKS WHATSOEVER
	public PlayerAnimationManager playerAnimationManager;

    //UI objects being managed
    public GameObject inventoryPanel;
    public GameObject slotPanel;
    public GameObject inventorySlot;
    public GameObject inventoryItem;

    public int slotAmount;

    public List<ItemData> inventoryItems = new List<ItemData>();
    public List<GameObject> inventorySlots = new List<GameObject>();

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
            inventorySlots.Add(newSlot);

            //add empty item to inventory to keep 1 to 1 ratio
            inventoryItems.Add(new ItemData());

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

    public bool AddItem(int id)
    {
        ItemData itemToAdd = database.FetchItemByID(id);

		if (itemToAdd != null) {
			int existingItemIndex = FindFirstItemInInventory (itemToAdd.ID);

			//stack if item is stackable and already in inventory (-1 = no existing item)
			if (itemToAdd.Stackable && existingItemIndex != -1)
			{
				UIItem existingUIItem = inventorySlots [existingItemIndex].GetComponentInChildren<UIItem> ();
				existingUIItem.amount++;
				existingUIItem.transform.GetComponentInChildren<Text> ().text = existingUIItem.amount.ToString ();

				return true;
			}
			else
			{ 
				//new item not present in inventory

				for (int i = 0; i < inventoryItems.Count; i++)
				{
					//if there is a free slot
					if (inventoryItems [i].ID == -1)
					{


						inventoryItems [i] = itemToAdd;

						//create visualization for item, position relative to parent slot
						GameObject itemObject = Instantiate (inventoryItem);
						UIItem itemUIObject = itemObject.gameObject.GetComponent<UIItem> ();
						itemUIObject.item = itemToAdd;

						//OPTIMISE: remember item original slot for Drag'n'Drop logic, a bit messy
						itemUIObject.slotID = i;
						itemUIObject.amount = 1;

						//positioning and hierarchy
						itemObject.transform.SetParent (inventorySlots [i].transform);
						itemObject.transform.localPosition = Vector2.zero;

						itemObject.GetComponent<Image> ().sprite = itemToAdd.Sprite;
						itemObject.name = itemToAdd.Title;

						//UGLY-ASS TEMPORARY CODE FIXME
						List<string> availableGuns = new List<string>{ "revolver", "shotgun", "rifle"};

						List<string> availableProjectiles = new List<string>{ "throwing_knife" };

						//if gun
						if (availableGuns.Contains(itemToAdd.Slug))
						{
							GunBehaviour gunLogic = itemObject.AddComponent<GunBehaviour> ();
							gunLogic.GunData = Resources.Load ("scriptable_objects/guns/" + itemToAdd.Slug) as GunData;

							gunLogic.InitialChecks ();
						}
						//if knifes
						else if (availableProjectiles.Contains(itemToAdd.Slug))
						{
							ThrowBehaviour throwLogic = itemObject.AddComponent<ThrowBehaviour> ();
							throwLogic.ProjectileData = Resources.Load ("scriptable_objects/projectiles/" + itemToAdd.Slug) as ProjectileData;

							throwLogic.InitialChecks ();
						}

						//if clothing
						if (itemToAdd.Type == ItemData.ItemType.clothing)
						{
							ClothingBehaviour clothingLogic = itemObject.AddComponent<ClothingBehaviour> ();
							clothingLogic.associatedItem = itemToAdd;
							clothingLogic.itemInstance = itemObject.GetComponent<UIItem> ();
							clothingLogic.spriteSwitcher = playerAnimationManager.bodySprites[(int)itemToAdd.Clothing];
						}

						if (itemToAdd.CanBeEquipped)
						{

							//equip this object (and de-equip similar type items if not clothing)
							EquipItem(itemUIObject, true);
						}

						//don't add the item to every free slot: get out of the loop!
						return true;

					}
				}

				return false;
			}
		}
		else
		{
			Debug.Log ("Couldn't find item to add (id = " + id);
			return false;
		}
    }

	public bool RemoveFirstItemInInventory(int id)
	{
		ItemData itemToRemove = database.FetchItemByID(id);

		if (itemToRemove != null)
		{
			int existingItemIndex = FindFirstItemInInventory (itemToRemove.ID);

			//-1 = no existing item
			if (existingItemIndex != -1)
			{
				UIItem existingUIItem = inventorySlots [existingItemIndex].GetComponentInChildren<UIItem> ();

				//if item is stackable and stack size is bigger than 1
				//TODO: this may lead to problems if we have several stacks of the same item (e.g. ammo)
				if (itemToRemove.Stackable && existingUIItem.amount > 1)
				{
					existingUIItem.amount--;

					existingUIItem.GetComponentInChildren<Text>().text = existingUIItem.amount.ToString();

				}
				else
				{
					//delete item from slot
					inventoryItems.Remove(inventoryItems[existingItemIndex]);
					GameObject.Destroy(inventorySlots[existingItemIndex].transform.GetChild(0).gameObject);
				}

				return true;
			}
			else //new item not present in inventory
			{
				Debug.Log ("Trying to remove an item that is not in the inventory" + itemToRemove.Slug);
				return false;
			}

		}
		else
		{
			Debug.Log ("Couldn't find item to remove (id = " + id);
			return false;
		}
	}

	public int FindFirstItemInInventory(int id) //this could return the slot number where the item was found, to simplify calculation
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].ID == id)
			{
				return i;
            }
        }

		//if no item was found (or ID is wrong), return null
        return -1;
    }

	public List<int> FindAllItemsOfTypeInInventory(ItemData.ItemType type)
	{
		List<int> results = new List<int>();

		for (int i = 0; i < inventoryItems.Count; i++)
		{
			if (inventoryItems[i].Type == type)
			{
				results.Add(i);
			}
		}

		//for safety, return null if list is null (meaning we didn't find anything of that type in the inventory)
		if (results.Count == 0)
		{
			Debug.Log ("NO SIMILAR TYPE FOUND");
			return null;
		}
		else
			return results;
	}

//	public void ToggleItemEquip(UIItem itemToToggleEquip)
//	{
//		itemToToggleEquip.inUse = !itemToToggleEquip.inUse;
//
//		//gross prototype code to change equipped item background color
//		if (itemToToggleEquip.transform.parent.gameObject.GetComponent<Image>().color != Color.yellow)
//			itemToToggleEquip.transform.parent.gameObject.GetComponent<Image>().color = Color.yellow;
//		else
//			itemToToggleEquip.transform.parent.gameObject.GetComponent<Image>().color = Color.white;
//	}

	public void EquipItem(UIItem itemToEquip, bool equip)
	{
		//gross prototype code to change equipped item background color
		if (equip)
		{
			//prior to equipping a new object of x type, de-equip all similar objects if not clothing
			if (itemToEquip.item.Type != ItemData.ItemType.clothing)
			{
				EquipAllItemsOfType (itemToEquip, false);
			}

			//color feedback
			itemToEquip.transform.parent.gameObject.GetComponent<Image> ().color = Color.yellow;
		}
		else
		{
			//color feedback
			itemToEquip.transform.parent.gameObject.GetComponent<Image> ().color = Color.white;
		}

		itemToEquip.equipped = equip;

		//debug code
		if (itemToEquip.item.Type == ItemData.ItemType.clothing)
		{
			itemToEquip.gameObject.GetComponent<ClothingBehaviour> ().OnToggleEquip ();
		}
	}

	public bool EquipAllItemsOfType(UIItem itemToEquip, bool equip)
	{
		List<int> similarTypeItemIndexes = FindAllItemsOfTypeInInventory (itemToEquip.item.Type);

		//if there are similar type items
		if (similarTypeItemIndexes != null)
		{
			//disable them
			foreach (int index in similarTypeItemIndexes) {
				UIItem existingUIItem = inventorySlots [index].GetComponentInChildren<UIItem> ();
				EquipItem (existingUIItem, equip);
			}

			return true;
		} else {
			Debug.Log ("No items of similar type found");
			return false;
		}
	}
}
