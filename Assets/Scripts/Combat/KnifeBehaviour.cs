using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KnifeBehaviour : MonoBehaviour {

    public GameObject throwingSource;
    public GameObject knifePrefab;

    public Text ammoTextUI;

	//gun sounds
	public AudioSource gunAudioSource;
	public AudioClip[] gunShotSounds;

    //gun firing, reloading delays
    public float fireRate;
    private bool canShoot = true;

	[Range(0.0f, 90.0f)]
	public float scatteringAngle;

	public float knifeSpeed;

    public float knifeMinDamage;
    public float knifeMaxDamage;

	void Update () {
	
		//shoot only if aiming and not reloading
		if (Input.GetKey(KeyCode.Mouse1) && Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
        {
            StartCoroutine(Shoot());
        }

    }

    IEnumerator Shoot()
    {
		//sound! PogChamp
		AudioClip randomSFX = gunShotSounds[Random.Range(0, gunShotSounds.Length)];
		gunAudioSource.PlayOneShot(randomSFX);

        GameObject newShot = Instantiate(knifePrefab);

        //calculating damage
        newShot.GetComponent<DamageBehaviour>().damage = Random.Range(knifeMinDamage, knifeMaxDamage);

        newShot.gameObject.transform.position = throwingSource.transform.position;

        //apply random scattering factor
        Vector3 scattering = new Vector3(0, 0, Random.Range(-scatteringAngle, scatteringAngle));
        newShot.gameObject.transform.Rotate(throwingSource.transform.eulerAngles + scattering);

        newShot.GetComponent<Rigidbody2D>().AddForce(-newShot.gameObject.transform.up * knifeSpeed);
        
        //todo: remove one knife from inventory

		//firing delay
        canShoot = false;
		yield return new WaitForSeconds(fireRate);
        
        canShoot = true;
    }
}
