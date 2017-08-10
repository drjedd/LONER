using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerPhysicsBehaviour : MonoBehaviour {

	//UNLESS WE DO CO-OP, THIS SHOULD BE A UNITY SINGLETON JUST FOR SAFETY

    public float walkingSpeed;
    public float runningSpeed;

    public bool WASDControls;

    private float verticalAxisValue;
    private float horizontalAxisValue;
    private Vector2 appliedForce;
    
    private Rigidbody2D attachedRigidbody2D;

    void Start ()
    {
        attachedRigidbody2D = GetComponent<Rigidbody2D>();

        if (attachedRigidbody2D == null)
        {
            Debug.LogError(gameObject.name + ": No Rigidbody2D component attached, required for movement to work. (Physics-based)");
        }
	}

    void Update()
    {
        //PROTO: switch between WASD style controls and Hotline Miami style controls
        if (Input.GetButtonDown("SwitchControls"))
        {
            WASDControls = !WASDControls;
        }

        if (WASDControls)
        {
            //read the axis(key presses)
            verticalAxisValue = Input.GetAxis("Vertical");
            horizontalAxisValue = Input.GetAxis("Horizontal");

            appliedForce = (verticalAxisValue * Vector2.up);
            appliedForce += (horizontalAxisValue * Vector2.right);


			//Debug.Log(verticalAxisValue + " + " + horizontalAxisValue + " = " + appliedForce);

			if (Input.GetButton("Run"))
            {
                appliedForce *= runningSpeed;
            }
            else
            {
                appliedForce *= walkingSpeed;
            }

            attachedRigidbody2D.AddForce(appliedForce);

        }
        else
        {

            float verticalInput = Input.GetAxis("Vertical");
            Vector2 verticalForce;

            if (Input.GetButton("Run"))
            {
                verticalForce = gameObject.transform.up * runningSpeed * -verticalInput;
            }
            else
            {
                verticalForce = gameObject.transform.up * walkingSpeed * -verticalInput;
            }

            GetComponent<Rigidbody2D>().AddForce(verticalForce);

        }

    }


}
