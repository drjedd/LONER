using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{

	public Animator headAnim;
	public Animator bodyAnim;


	public SpriteSwitcher hat;
	public SpriteSwitcher hair;
	public SpriteSwitcher mustache;
	public SpriteSwitcher beard;
	public SpriteSwitcher scarf;
	public SpriteSwitcher head;

	public SpriteSwitcher jacket;
	public SpriteSwitcher shirt;
	public SpriteSwitcher pants;
	public SpriteSwitcher body;

	void Start ()
    {
		bodyAnim.SetFloat("animationOffset", 0.07f);

    }
	
	void Update ()
    {
		#region CHARACTER CUSTOMISATION SHORTCUTS
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{

			head.NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{

			beard.NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{

			scarf.NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{

			mustache.NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{

			hair.NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{

			hat.NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{

			pants.NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{

			shirt.NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{

			jacket.NextSprite();
		}
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{

			body.NextSprite();
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
				//Elegant solution by Zas_ in Twitch chat:
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
