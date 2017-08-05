using UnityEngine;
using System.Collections;

public class SlidingUIBehaviour : MonoBehaviour {

    private Animator attachedAnimator;
    private bool animationToggle;

	void Start () {
        //security checks
        attachedAnimator = GetComponent<Animator>();
	    if (attachedAnimator == null)
        {
            Debug.LogError(gameObject.name + ": There must be an Animator component attached to this game object for the sliding animation to work.");
        }
	}
	
	void Update () {

        //TODO: use event based messaging system instead. Clean code for reusability
        if (Input.GetButtonDown("Inventory"))
        {
            animationToggle = !animationToggle;
            attachedAnimator.SetBool("inventoryDisplay", animationToggle);
        }
	}
}
