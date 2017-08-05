using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShotgunBehaviour : MonoBehaviour {

    public GameObject gunTip;
    public GameObject bulletPrefab;

    public Text ammoTextUI;
    public Text reloadingTextUI;

    //gun firing, reloading delays
    public float fireRate;
    public float reloadRate;
    private bool canShoot = true;

    public int magazineSize;
    private int magazineCurrentSize;

    public int scatterSize;
    [Range(0.0f, 90.0f)]
    public float scatteringAngle;
    public float bulletSpeed;

    public float bulletMinDamage;
    public float bulletMaxDamage;

    // Use this for initialization
    void Start () {

        //implying we start with a full magazine
        magazineCurrentSize = magazineSize;
        ammoTextUI.text = "Bullets: " + magazineCurrentSize.ToString();

        reloadingTextUI.enabled = false;


    }
	
	void Update () {
	
		//shoot only if aiming and not reloading
		if (Input.GetKey(KeyCode.Mouse1) && Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
        {
            StartCoroutine(Shoot());
        }

    }

    IEnumerator Shoot()
    {
        //shotgun mechanics: multiple bullets at once
        for (int i = 0; i < scatterSize; i++)
        {

            GameObject newShot = Instantiate(bulletPrefab);

            //calculating damage
            newShot.GetComponent<DamageBehaviour>().damage = Random.Range(bulletMinDamage, bulletMaxDamage);

            newShot.gameObject.transform.position = gunTip.transform.position;

            //apply random scattering factor
            Vector3 scattering = new Vector3(0, 0, Random.Range(-scatteringAngle, scatteringAngle));
            newShot.gameObject.transform.Rotate(gunTip.transform.eulerAngles + scattering);

            newShot.GetComponent<Rigidbody2D>().AddForce(-newShot.gameObject.transform.up * bulletSpeed);

        }
        
        magazineCurrentSize--;
        ammoTextUI.text = "Bullets: " + magazineCurrentSize.ToString();

        if (magazineCurrentSize == 0)
        {
            //reloading
            canShoot = false;
            reloadingTextUI.enabled = true;
            yield return new WaitForSeconds(reloadRate);
            reloadingTextUI.enabled = false;
            magazineCurrentSize = magazineSize; //no management of ammo so far
            ammoTextUI.text = "Bullets: " + magazineCurrentSize.ToString();
        }
        else
        {
            //firing delay
            canShoot = false;
            yield return new WaitForSeconds(fireRate);
        }
        
        canShoot = true;
    }
}
