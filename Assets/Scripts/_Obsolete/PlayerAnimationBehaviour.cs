using UnityEngine;
using System.Collections;

public class PlayerAnimationBehaviour : MonoBehaviour
{

	public AnimationCurve bodyAnimationCurve;
	public AnimationCurve headAnimationCurve;

    public Animator attachedAnimator;

	void Start ()
    {
        if (attachedAnimator == null)
        {
            Debug.LogError(gameObject.name + ": No Animator component attached, required for animation to work.");
        } 
    }
	
	void Update ()
    {

		if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (Input.GetButton("Run"))
            {
                attachedAnimator.SetBool("isRunning", true);
                attachedAnimator.SetBool("isWalking", false);
            }
            else
            {
                attachedAnimator.SetBool("isWalking", true);
                attachedAnimator.SetBool("isRunning", false);
            }
        }
        else
        {
            attachedAnimator.SetBool("isWalking", false);
            attachedAnimator.SetBool("isRunning", false);
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
		attachedAnimator.SetFloat("lookRotation", angle);
	}
}
