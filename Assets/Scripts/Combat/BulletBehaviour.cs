using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    public GameObject bulletTrail;

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
		Debug.Log("TRIGGERED HotPokket (BulletBehaviour");
        if (collision.collider.gameObject.tag != "ShotgunBullet")
        {
			//GetComponent<Rigidbody2D>().Sleep();
			GetComponent<Collider2D>().enabled = false;
			Destroy(gameObject, Const.BULLET_LIFE_TIME);
		}

		moving = false;
    }

	private void FixedUpdate()
	{
		if (moving) {
			transform.position = transform.forward * bulletSpeed * Time.deltaTime;
		}
	}
}
