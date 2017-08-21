using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunBehaviour : MonoBehaviour {

	//scriptable object instance, hence the capitalisation
	public GunData GunData;

	public bool inUse = false;
	private bool canShoot = true;
	private bool reloading = true;

	private InventoryManager inventoryManager;

	//source of the projectile (limited to right hand for now)
	private GameObject projectileSource;

	//camera shake curve
	private MainCameraBehaviour mainCamera;
	private float cameraShakeCurveEndTime;

	//gun sounds source
	private AudioSource gunAudioSource;
	
	private int magazineCurrentSize;


	public void InitialChecks()
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

		//security checks: look for camera in scene, make sure proper component is attached
		GameObject mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");

		if (mainCameraObject == null)
		{
			Debug.LogError(gameObject.name + ": Can't find the Main Camera. It is necessary for item pick-up behaviour.");
		}
		else
		{
			mainCamera = mainCameraObject.GetComponent<MainCameraBehaviour>();

			if (mainCamera == null)
			{
				Debug.LogError(gameObject.name + ": Can't find the Main Camera Behaviour component attached to the Inventory Manager game object. It is necessary for item pick-up behaviour.");
			}
		}

		//find the last keyframe of the shake curve
		Keyframe lastKeyframe = GunData.cameraShakeCurve[GunData.cameraShakeCurve.length - 1];
		cameraShakeCurveEndTime = lastKeyframe.time;


		//TEMPORARY, BUG-PRONE
		projectileSource = GameObject.FindGameObjectWithTag("ProjectileSource");


		gunAudioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();

		//debug: start will full magazine
		magazineCurrentSize = GunData.magazineSize;
	}

	void Update () {

		//enable gun only if item is equipped
		inUse = GetComponent<UIItem>().inUse;

		//shoot only if aiming and not reloading
		if (inUse && Input.GetKey(KeyCode.Mouse1) && Input.GetKeyDown(KeyCode.Mouse0) && canShoot && inventoryManager.CheckIfItemIsInInventory(4))
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
		//sound! PogChamp
		if ( GunData.gunShotSounds.Length > 0 ) {
			AudioClip randomSFX = GunData.gunShotSounds[Random.Range(0, GunData.gunShotSounds.Length)];
			gunAudioSource.PlayOneShot(randomSFX);
		}

		//apply camera shake
		mainCamera.cameraShakeCurve = GunData.cameraShakeCurve;
		mainCamera.cameraShakeCurveEndTime = cameraShakeCurveEndTime;
		mainCamera.cameraShakeCurrentTime = 0;

		//do... while loop so the code is ran at least once no matter the condition
		int i = 0;

		do { 
			GameObject newShot = Instantiate(GunData.projectilePrefab);

			//calculating damage
			newShot.GetComponent<DamageBehaviour>().damage = Random.Range(GunData.projectileMinDamage, GunData.projectileMaxDamage);

			newShot.gameObject.transform.position = projectileSource.transform.position;

			//define bullet base rotation
			newShot.gameObject.transform.up =  Camera.main.ScreenToWorldPoint(Input.mousePosition) - newShot.gameObject.transform.position;


			//apply random scattering factor
			Vector3 scattering = new Vector3(0, 0, Random.Range(-GunData.scatteringAngle, GunData.scatteringAngle));

			Debug.Log("Scattering: " + scattering.z);
			newShot.gameObject.transform.Rotate(scattering);

			//newShot.gameObject.transform.LookAt(mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition));

			newShot.GetComponent<Rigidbody2D>().AddForce((newShot.gameObject.transform.up) * GunData.projectileSpeed);

			Physics2D.IgnoreCollision(newShot.GetComponent<Collider2D>(), Player.Instance.gameObject.GetComponent<Collider2D>(), true);



			//BulletBehaviour bulletBehaviour = newShot.GetComponent<BulletBehaviour>();

			//bulletBehaviour.bulletSpeed = GunData.projectileSpeed;

			////debug: assigning Player as default owner
			//bulletBehaviour.owner = Player.Instance.gameObject;

			// (poor) ammo management
			magazineCurrentSize--;
			inventoryManager.RemoveItem(4);

			i++;

		} while ( i < GunData.ammoSpentPerShot);

		if (magazineCurrentSize <= 0)
		{
			//reload if possible
			canShoot = false;
			reloading = true;

			yield return new WaitForSeconds(GunData.reloadRate);


			//TODO: proper ammo reloading mechanics
			magazineCurrentSize = GunData.magazineSize;
			reloading = false;
		}
		else
		{
			//firing delay
			canShoot = false;
			yield return new WaitForSeconds(GunData.fireRate);
		}
		
		canShoot = true;
    }
}
