using UnityEngine;
using System.Collections;

public class DamageBehaviour : MonoBehaviour {

	//the health component of the recipient of the damage
    private HealthBehaviour collisionTarget;

    public float damage;
    private bool damageSuccessful;

    void Start()
    {
        //make sure there is an Collider2D attached
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogError(gameObject.name + ": No Collider2D component attached, required for damage dealing to work. (Collision-based)");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        collisionTarget = collision.collider.gameObject.GetComponent<HealthBehaviour>();

        if (collisionTarget != null)
        {
            damageSuccessful = collisionTarget.TryToDamage(damage);
                
            if (damageSuccessful)
            {
                //Debug.Log(gameObject.name + " dealt " + damage + " damage to " + collisionTarget.gameObject.name);
            }
            else
            {
                Debug.Log(gameObject.name + " failed to damage " + collisionTarget.gameObject.name + ". Target might be invincible.");
            }
        }
    }
}
