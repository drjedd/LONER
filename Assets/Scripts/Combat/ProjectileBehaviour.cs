using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {

	public bool rotating = true;
	public float rotationSpeed;
	public bool rotateClockwise;

	// Use this for initialization
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
			this.gameObject.transform.Rotate(new Vector3(0, 0, (rotationSpeed * Time.deltaTime)));
		}
		
	}
}
