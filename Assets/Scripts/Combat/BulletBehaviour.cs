using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {

    public GameObject bulletTrail;

    void Start ()
    {
        //bullet trail debug
        bulletTrail.GetComponent<TrailRenderer>().sortingLayerName = "Foreground";
        bulletTrail.GetComponent<TrailRenderer>().sortingOrder = 100;
    }

    void Awake ()
    {
        Destroy(gameObject, 3);
    }
	
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.tag != "ShotgunBullet")
        {
			GetComponent<Rigidbody2D>().Sleep();
			GetComponent<Collider2D>().enabled = false;
			Destroy(gameObject, 3);
		}
    }
}
