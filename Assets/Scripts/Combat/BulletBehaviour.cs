﻿using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    public GameObject bulletTrail;
	public GameObject owner;

	public float bulletSpeed;
	public bool moving = true;

    void Start ()
    {
        //bullet trail debug
        bulletTrail.GetComponent<TrailRenderer>().sortingLayerName = "Foreground";
        bulletTrail.GetComponent<TrailRenderer>().sortingOrder = 100;

	}

    void Awake ()
    {
        Destroy(gameObject, Const.BULLET_LIFE_TIME);
    }
	
    void OnCollisionEnter2D(Collision2D collision)
    {
		Debug.Log("Bullet collided with: " + collision.collider.gameObject.name);
		//do not collide with the person who fired
		if ( collision.collider.gameObject != owner ) { 

			//debug for shotgun bullets
			if ( collision.collider.gameObject.tag != "ShotgunBullet" ) {
				
				//GetComponent<TrailRenderer>().

				GetComponent<Rigidbody2D>().Sleep();
				GetComponent<Collider2D>().enabled = false;
				Destroy(gameObject, Const.BULLET_LIFE_TIME);
			}
		}

		moving = false;
    }

	//private void Update() {
	//	if ( moving ) {
	//		transform.position += transform.up * bulletSpeed * Time.deltaTime;
	//	}

	//}
}
