using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

	public bool rotating = false;
	public bool slowingDown = false;
	public float slowDownFactor = 0.9f;
	public float rotationSpeed;
	public bool rotateClockwise;

	void Awake () {

		Destroy(gameObject, Const.BULLET_LIFE_TIME);

		if (rotateClockwise)
		{
			rotationSpeed = -rotationSpeed;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (rotating)
		{
			if (slowingDown)
			{
				rotationSpeed = rotationSpeed * slowDownFactor * Time.deltaTime;
			}

			this.gameObject.transform.Rotate(new Vector3(0, 0, (rotationSpeed * Time.deltaTime)));
		}
		
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Projectile collided with: " + collision.collider.gameObject.name);
		
		Destroy(gameObject, 1f); //DEBUG
		slowingDown = true;
	}
}
