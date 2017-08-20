using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSwitcher : MonoBehaviour {

	//TODO: there may be a better way to manage the empty sprite possibility. But this works, so... fuck it.

	public Sprite[] spriteArray;
	public int currentSpriteIndex = 0;

	public bool canBeEmpty = false;
	public bool isEmpty = false;

	// Use this for initialization
	void Start () {
		if (canBeEmpty && isEmpty)
		{
			ChangeSprite(-1);
			currentSpriteIndex = -1;
		}
		else
		{
			ChangeSprite(currentSpriteIndex);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void ChangeSprite(int desiredSpriteIndex)
	{
		//if the index makes sense and is valid
		if (desiredSpriteIndex >= 0 && desiredSpriteIndex < spriteArray.Length)
		{
			//if it was empty
			if (isEmpty)
			{
				GetComponent<SpriteRenderer>().enabled = true;
				isEmpty = false;
			}

			GetComponent<SpriteRenderer>().sprite = spriteArray[desiredSpriteIndex];
			currentSpriteIndex = desiredSpriteIndex;
		}
		else if (desiredSpriteIndex == -1 && canBeEmpty)
		{
			GetComponent<SpriteRenderer>().enabled = false;
			isEmpty = true;
		}
		else
		{
			Debug.Log("The desired sprite index is invalid or out of array bounds");
		}
	}

	public void NextSprite()
	{

		currentSpriteIndex++;

		//if the index makes sense and is valid
		if (currentSpriteIndex == spriteArray.Length)
		{
			if (canBeEmpty && !isEmpty)
			{
				isEmpty = true;
				ChangeSprite(-1);
				currentSpriteIndex = -1;
			}
			else
			{
				currentSpriteIndex = 0;
			}
		}

		ChangeSprite(currentSpriteIndex);
	}
}
