using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    //public float walkingSpeed;
    //public float runningSpeed;

    void FixedUpdate()
    {
        //ROTATION
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, transform.position - mousePosition);
        transform.rotation = rotation;

        //Below: original movement script, DEPRECATED, replaced by WASD-style movement in new PlayerPhysicsBehaviour script

        //float verticalInput = Input.GetAxis("Vertical");
        //Vector2 verticalForce;

        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    verticalForce = gameObject.transform.up * runningSpeed * verticalInput;
        //}
        //else
        //{
        //    verticalForce = gameObject.transform.up * walkingSpeed * verticalInput;
        //}

        //GetComponent<Rigidbody2D>().AddForce(verticalForce);
    }
}