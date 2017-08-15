using UnityEngine;
using System.Collections;

/*
* PUSHBACK BEHAVIOUR
* ---
* pushes back collision target, if applicable
*/

public class KnockbackBehaviour : MonoBehaviour {
    
    private Collider2D localCollider;
    private Vector2 localColliderCenter;
    private Vector2 targetColliderCenter;
    private PhysicsBehaviour targetPhysicsBehaviour;
    
    public float knockbackStrength;
    private Vector2 knockbackDirection;
    private bool knockbackSuccessful;

    void Start()
    {
        //make sure there is an Collider2D attached
        localCollider = GetComponent<Collider2D>();

        if (localCollider == null)
        {
            Debug.LogError(gameObject.name + ": No Collider2D component attached, required for push-back to work. (Collision-based)");
        }

        //calculate the object center based on collider, taking offset into account (meaning sprite pivot point doesn't have to be where the collider center is)
        localColliderCenter = CalculateColliderCenter(localCollider);
    }

    //VERIFY: is OnCollisionEnter the best function to call? There's also -Stay and -Exit
    void OnCollisionEnter2D(Collision2D collision)
    {
		Collider2D targetCollider = collision.collider;

        targetPhysicsBehaviour = targetCollider.gameObject.GetComponent<PhysicsBehaviour>();
        if (targetPhysicsBehaviour != null)
        {
            /*IMPLEMENT: calculate push-back direction
            *
            * To calculate the angle/direction, draw vector from pushback object centre to collision point
            * 
            * Vector2.Angle? definitely not any object rotation: must work on static objects too, like a cactus
            * 
            * QUESTION: game object transform centre OR collider centre?
            * imagine a sprite with complex coumpound colliders, collider centre seems to make more sense
            * 
            */
            targetColliderCenter = CalculateColliderCenter(targetCollider);

            knockbackDirection = targetColliderCenter - localColliderCenter;
            knockbackDirection.Normalize();
            
            //physics push-back effect
            knockbackSuccessful = targetPhysicsBehaviour.Knockback(knockbackDirection, knockbackStrength);

            if (knockbackSuccessful)
            {
                //Debug.Log(gameObject.name + " pushed " + targetCollider.gameObject.name + ". Vector: " + knockbackDirection.ToString());
            }
            else
            {
                //Debug.Log(gameObject.name + " failed to push " + targetCollider.gameObject.name);
            }
        }
    }

    private Vector2 CalculateColliderCenter(Collider2D collider)
    {
        Vector2 colliderCenter = new Vector2((collider.transform.position.x + collider.offset.x), (collider.transform.position.y + collider.offset.y));
        return colliderCenter;
    }
}
