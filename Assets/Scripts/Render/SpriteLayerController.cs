using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerController : MonoBehaviour
{
	public bool useAnArray;
	//an array is needed for more complex objects manipulating several sprites (e.g. player)
	public SpriteRenderer[] spritesArray;

	void Start()
	{
		//if the array is bigger than one SpriteRenderer
		if (!useAnArray)
		{
			SpriteRenderer attachedSpriteRenderer = GetComponent<SpriteRenderer>();

			//quick check that we do have an SpriteRenderer indeed
			if (attachedSpriteRenderer == null)
			{
				Debug.Log(gameObject.name + ": No Sprite Renderer component found on this object");
				return;
			}

			spritesArray = new SpriteRenderer[1];
			spritesArray[0] = attachedSpriteRenderer;
		}
	}

	private void Update()
	{
		for (int i = 0; i < spritesArray.Length; i++)
		{
			//assign sprite sorting order depending on Y position (preface with minus to get top-most objects to appear below
			spritesArray[i].sortingOrder = Mathf.RoundToInt((-transform.position.y * Const.SORTING_ORDER_MULTIPLIER) - i);
			//Debug.Log(spritesArray[i].sortingOrder);
		}

		//foreach (SpriteRenderer sprite in spritesArray)
		//{
		//	//assign sprite sorting order depending on Y position (preface with minus to get top-most objects to appear below
		//	sprite.sortingOrder = Mathf.RoundToInt((-transform.position.y * Const.SORTING_ORDER_MULTIPLIER));
		//	Debug.Log(sprite.sortingOrder);
		//}
	}
}
