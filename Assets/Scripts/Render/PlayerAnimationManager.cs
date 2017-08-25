using UnityEngine;
using System.Collections;

/* 
 * PLAYER ANIMATION BEHAVIOUR
 * 
 * Defines player sprites and animations
 * 
 * Contains debug code for quick customisation through numbers 1-9
 * 
 */

public class PlayerAnimationManager : MonoBehaviour
{

	public Animator headAnim;
	public Animator bodyAnim;


	public SpriteSwitcher[] bodySprites;

	void Start ()
    {
		bodyAnim.SetFloat("animationOffset", 0.07f);

    }
	
	void Update ()
    {
		#region CHARACTER CUSTOMISATION SHORTCUTS
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{

			bodySprites[0].NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{

			bodySprites[1].NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{

			bodySprites[2].NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{

			bodySprites[3].NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{

			bodySprites[4].NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{

			bodySprites[5].NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{

			bodySprites[6].NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{

			bodySprites[7].NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{

			bodySprites[8].NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{

			bodySprites[9].NextSprite();
		}
		#endregion

		if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{
			if (Input.GetButton("Run"))
			{
				headAnim.SetBool("isRunning", true);
				bodyAnim.SetBool("isRunning", true);

				headAnim.SetBool("isWalking", false);
				bodyAnim.SetBool("isWalking", false);

			}
			else
			{
				headAnim.SetBool("isWalking", true);
				bodyAnim.SetBool("isWalking", true);

				headAnim.SetBool("isRunning", false);
				bodyAnim.SetBool("isRunning", false);
			}
		}
		else
		{
			headAnim.SetBool("isWalking", false);
			bodyAnim.SetBool("isWalking", false);

			headAnim.SetBool("isRunning", false);
			bodyAnim.SetBool("isRunning", false);

		}

		//if aiming with right click, character looks at mouse cursor
		if (Input.GetKey(KeyCode.Mouse1))
		{
			LookAtAngle(transform.rotation.eulerAngles.z);
		}
		else
		{
			//otherwise calculate look angle based on WASD input
			Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			if (movementInput != Vector2.zero) //if no axis input is detected, do not change looking angle
			{
				//Elegant solution by Zas_ in Twitch chat (<3):
				float inputAngle;
				if (movementInput.x >= 0)
				{
					inputAngle = Vector2.Angle(new Vector2(0, -1), movementInput);
				}
				else
				{
					inputAngle = Vector2.Angle(new Vector2(0, 1), movementInput);
					inputAngle += 180;
				}

				LookAtAngle(inputAngle);
			}

		}
	}


	public void LookAtAngle(float angle)
	{
		headAnim.SetFloat("lookRotation", angle);
		bodyAnim.SetFloat("lookRotation", angle);
	}
}
