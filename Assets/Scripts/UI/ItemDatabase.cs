using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

//MOST OF THE CODE FROM AwfulMedia inventory tutorial on YouTube

public class ItemDatabase : MonoBehaviour {
    private List<Item> database = new List<Item>();
    private JsonData itemData;

    void Start ()
    {
        itemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
        ConstructItemDatabase();
    }

    public Item FetchItemByID (int id)
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
            database.Add(new Item((int)itemData[i]["id"], itemData[i]["slug"].ToString(),
                (bool)itemData[i]["stackable"], itemData[i]["title"].ToString(), itemData[i]["description"].ToString(), (double)itemData[i]["damage"]));
        }
    }
}

public class Item
{
    //properties
    public int ID { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }
    public bool Stackable { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public double Damage { get; set; }

    //empty constructor in case of problem or item removal (id being -1 it won't get parsed by a for loop
    public Item()
    {
        this.ID = -1;
    }

    //default constructor
    public Item (int id, string slug, bool stackable, string title, string description, double damage)
    {
        this.ID = id;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("sprites/items/" + slug + "/" + slug + "_inventory");
        this.Stackable = stackable;
        this.Title = title;
        this.Description = description;
        this.Damage = damage; 
    }

}
