using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

        //toolTip = GameObject.Find("Inventory ToolTip");
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
