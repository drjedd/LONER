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

	private InventoryManager inventoryManager;

	private void Start()
	{
		//security checks: look for inventory manager in scene, make sure proper component is attached
		GameObject inventoryManagerObject = GameObject.FindGameObjectWithTag("InventoryManager");

		if (inventoryManagerObject == null)
		{
			Debug.LogError(gameObject.name + ": Can't find the Inventory Manager game object. It is necessary for item pick-up behaviour.");
		}
		else
		{
			inventoryManager = inventoryManagerObject.GetComponent<InventoryManager>();

			if (inventoryManager == null)
			{
				Debug.LogError(gameObject.name + ": Can't find the InventoryManager component attached to the Inventory Manager game object. It is necessary for item pick-up behaviour.");
			}
		}
	}

	void Update () {

		//shoot only if aiming and not reloading
		if (Input.GetKey(KeyCode.Mouse1) && Input.GetKeyDown(KeyCode.Mouse0) && canShoot && inventoryManager.CheckIfItemIsInInventory(4))
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
		//sound! PogChamp
		AudioClip randomSFX = gunShotSounds[Random.Range(0, gunShotSounds.Length)];
		gunAudioSource.PlayOneShot(randomSFX);

		//apply camera shake
		FollowCameraWithBoundingBehaviour shakeCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowCameraWithBoundingBehaviour>();
		shakeCamera.cameraShakeCurrentTime = 0;

        GameObject newShot = Instantiate(knifePrefab);

        //calculating damage
        newShot.GetComponent<DamageBehaviour>().damage = Random.Range(knifeMinDamage, knifeMaxDamage);

        newShot.gameObject.transform.position = throwingSource.transform.position;

        //apply random scattering factor
        Vector3 scattering = new Vector3(0, 0, Random.Range(-scatteringAngle, scatteringAngle));
        newShot.gameObject.transform.Rotate(throwingSource.transform.eulerAngles + scattering);

        newShot.GetComponent<Rigidbody2D>().AddForce(newShot.gameObject.transform.up * knifeSpeed);

		//todo: remove one knife from inventory
		inventoryManager.RemoveItem(4);

		//firing delay
        canShoot = false;
		yield return new WaitForSeconds(fireRate);
        
        canShoot = true;
    }
}
