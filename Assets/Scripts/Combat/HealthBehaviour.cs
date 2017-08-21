using UnityEngine;
using System.Collections;

public class HealthBehaviour : MonoBehaviour {

    public float health = 0;
    public float regenerationRate = 0;

    //OPTIMISE: delay between damage, probably depending on animation
    public float postDamageDelay = 0;
    public bool canTakeDamage = true;

	void Update () {
        //regen code, including time to mimic fixedupdate
	}

    public bool TryToDamage(float appliedDamage)
    {
        if (canTakeDamage)
        {

            health -= appliedDamage;

            //death if applicable
            if (health <= 0)
            {
                Death();
            }
            
            return true;
        }
        else
        {
            return false;
        }
    }

    void Death ()
    {
		//TODO: real death event + animation
        Debug.Log(this.name + " died (temp death script).");
        if (gameObject.tag != "Player")
        { 
            Destroy(gameObject);
        }
    }
}
