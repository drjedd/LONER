﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

//MOST OF THE CODE FROM AwfulMedia inventory tutorial on YouTube

/* Two classes: ItemDatabase and ItemData (simply named "Item" in original tutorial) */

public class ItemDatabase : MonoBehaviour {
    private List<ItemData> database = new List<ItemData>();
    private JsonData itemData;

    void Start ()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();
		//ListItemDatabase ();
    }

    public ItemData FetchItemByID (int id)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].ID == id)
            { 
                return database[i];
            }
        }

        return null;
    }

    void ConstructItemDatabase ()
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            database.Add(new ItemData((int)itemData[i]["id"], itemData[i]["slug"].ToString(), (int)itemData[i]["type"],
				(bool)itemData[i]["stackable"], (bool)itemData[i]["canBeEquipped"], itemData[i]["title"].ToString(), itemData[i]["description"].ToString(), (double)itemData[i]["damage"],
				(int)itemData[i]["clothingType"], (int)itemData[i]["clothingSpriteIndex"]));
        }
    }

	void ListItemDatabase ()
	{
		Debug.Log ("ITEM DATABASE LIST:");

		foreach (ItemData item in database)
		{
			Debug.Log (item.ID + " // slug: " + item.Slug + " // type: " + item.Type + " // clothing type: " + item.Clothing + " // clothing sprite index: " + item.ClothingSpriteIndex);
		}
	}
}

public class ItemData
{
    //properties
    public int ID { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }
    public bool Stackable { get; set; }
	public bool CanBeEquipped { get; set; }

	public enum ItemType {
		undefined, weapon, projectile, ammunition, clothing, consumable, quest_item
	}
	public ItemType Type { get; set; }

	public string Title { get; set; }
    public string Description { get; set; }
    public double Damage { get; set; }

	//clothing sprite management
	public enum ClothingType {
		Hat, Hair, Mustache, Beard, Head, Scarf, Jacket, Shirt, Pants, Body
	}
	public ClothingType Clothing { get; set; }
	public int ClothingSpriteIndex { get; set; }

    //empty constructor in case of problem or item removal (id being -1 it won't get parsed by a for loop
    public ItemData()
    {
        this.ID = -1;
    }

    //default constructor
	public ItemData (int id, string slug, int type, bool stackable, bool canBeEquipped, string title, string description, double damage, int clothing, int clothingSpriteIndex)
    {
        this.ID = id;
        this.Slug = slug;
		this.Type = (ItemType) type;
		this.Sprite = Resources.Load<Sprite>("sprites/items/" + (ItemType)type + "/" + slug + "/" + slug + "_inventory");
        this.Stackable = stackable;
		this.CanBeEquipped = canBeEquipped;
		this.Title = title;
        this.Description = description;
        this.Damage = damage; 

		this.Clothing = (ClothingType) clothing;
		this.ClothingSpriteIndex = clothingSpriteIndex;
    }
}
