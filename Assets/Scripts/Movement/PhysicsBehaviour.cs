using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PhysicsBehaviour : MonoBehaviour {

    public bool knockable = false;

    private Collider2D attachedCollider;
    private Rigidbody2D attachedRigidbody;
    
	void Start () {

		//check for required components on Game Object:

		attachedCollider = GetComponent<Collider2D>();

		if (attachedCollider == null)
		{
			Debug.Log(gameObject.name + ": No Collider2D component attached, required for some functionality (e.g. push-back) to work. (Collision-based)");
		}

		attachedRigidbody = GetComponent<Rigidbody2D>();

		if (attachedRigidbody == null)
		{
			Debug.LogError(gameObject.name + ": No Rigidbody2D component attached, required for all functionality to work. (Physics-based)");
		}
	}

    public bool Knockback(Vector2 direction, float strength = 1)
    {
        if (knockable)
        {
            //VERIFY: push logic
            attachedRigidbody.AddForce(direction * strength, ForceMode2D.Impulse);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void Move(Vector2 direction, float strength = 1)
    {
        attachedRigidbody.AddForce(direction * strength);
    }
}
