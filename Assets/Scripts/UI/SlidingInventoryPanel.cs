using UnityEngine;
using System.Collections;

public class SlidingInventoryPanel : MonoBehaviour {

    private Animator attachedAnimator;
    private bool slidingIn;

	private MainCameraBehaviour mainCamera;

	void Start () {

        //security checks
        attachedAnimator = GetComponent<Animator>();
	    if (attachedAnimator == null)
        {
            Debug.LogError(gameObject.name + ": There must be an Animator component attached to this game object for the sliding animation to work.");
        }

		//security checks: look for camera in scene, make sure proper component is attached
		GameObject mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");

		if (mainCameraObject == null)
		{
			Debug.LogError(gameObject.name + ": Can't find the Main Camera. It is necessary for item pick-up behaviour.");
		}
		else
		{
			mainCamera = mainCameraObject.GetComponent<MainCameraBehaviour>();

			if (mainCamera == null)
			{
				Debug.LogError(gameObject.name + ": Can't find the Main Camera Behaviour component attached to the Inventory Manager game object. It is necessary for item pick-up behaviour.");
			}
		}
	}
	
	void Update () {

        //TODO: use event based messaging system instead. Clean code for reusability
        if (Input.GetButtonDown("Inventory"))
        {
            slidingIn = !slidingIn;
            attachedAnimator.SetBool("inventoryDisplay", slidingIn);

			if (slidingIn)
			{
				mainCamera.positionOffset = new Vector2(-0.6f, 0f);
			}
			else
			{
				mainCamera.positionOffset = Vector2.zero;
			}
		}
	}
}
