using UnityEngine;
using System.Collections;

public class ZombieBehaviour : MonoBehaviour {

    public float aggroDistance;
    public float speed;

    public Animator attachedAnimator;

    void Start()
    {
        if (attachedAnimator == null)
        {
            Debug.LogError(gameObject.name + ": No Animator component attached, required for animation to work.");
        }
    }

    void FixedUpdate ()
    {
		//if no player in the scene, don't bother (dirty)
		if (Player.Instance != null)
		{
			if (Vector3.Distance(Player.Instance.transform.position, transform.position) <= aggroDistance)
			{
				//math from Unity Live Training Top Down Games
				float z = Mathf.Atan2((Player.Instance.transform.position.y - transform.position.y), (Player.Instance.transform.position.x - transform.position.x)) * Mathf.Rad2Deg + 90;

				transform.eulerAngles = new Vector3(0, 0, z);

				GetComponent<Rigidbody2D>().AddForce(-gameObject.transform.up * speed);

				attachedAnimator.SetFloat("lookRotation", transform.rotation.eulerAngles.z);

				attachedAnimator.SetBool("isWalking", true);
			}
			else
			{
				attachedAnimator.SetBool("isWalking", false);
			}
		}

	}
    
}
