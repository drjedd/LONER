using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/* 
 * TOOL TIP BEHAVIOUR
 * 
 * Displays a tool tip with information on the item being hovered by the mouse
 * 
 * TODO: GameObject.Find() is so bad. Unity Singleton maybe? Is there ever going to be more than one tooltip on the screen at a time?
 * 
 */

public class ToolTipBehaviour : MonoBehaviour {

    public Color itemTitleColor;
    public Color itemDamageColor;
    public Color itemDescriptionColor;

    private ItemData item;
    private string itemData;
    public GameObject toolTip;

    void Start ()
    {
        //find ACTIVE ToolTip and deactivate it straight away: Find() will not look for the gameObject if it is already deactivated
        toolTip = GameObject.Find("Inventory ToolTip");
        toolTip.SetActive(false);
    }

    void Update ()
    {
		//shutdown for debug

        //if (toolTip.activeSelf)
        //{
        //    toolTip.transform.position = Input.mousePosition;
        //}
    }
    
    public void Activate (ItemData item)
    {
        this.item = item;
        ConstructDataString();
        toolTip.SetActive(true);
    }

    public void Deactivate()
    {
        toolTip.SetActive(false);
    }

    public void ConstructDataString ()
    {
        itemData = "<color=#" + ColorUtility.ToHtmlStringRGB(itemTitleColor) + "><b>" + item.Title + "</b></color>";
        itemData += "\n\n<color=#" + ColorUtility.ToHtmlStringRGB(itemDamageColor) + ">Damage : " + item.Damage + "</color>";
        itemData += "\n\n<color=#" + ColorUtility.ToHtmlStringRGB(itemDescriptionColor) + "><i>" + item.Description + "</i></color>";
        toolTip.transform.GetChild(0).GetComponent<Text>().text = itemData;
    }
}
