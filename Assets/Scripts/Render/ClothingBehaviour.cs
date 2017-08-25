using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingBehaviour : MonoBehaviour
{

	// references set by inventorymanager
	public SpriteSwitcher spriteSwitcher;
	public ItemData associatedItem;
	public UIItem itemInstance;

	// Update is called once per frame
	public void OnToggleEquip()
	{
		if (itemInstance.equipped)
		{
			//Debug.Log ((int)associatedItem.Clothing);
			spriteSwitcher.ChangeSprite (associatedItem.ClothingSpriteIndex);
		}
		else
		{
			spriteSwitcher.ChangeSprite (-1);
		}
	}
}
