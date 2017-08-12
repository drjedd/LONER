using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunBehaviour : MonoBehaviour {

	public bool inUse = true;
    public GameObject projectilePrefab;

	//source of the projectile, usually the right hand
	private GameObject projectileSource;

	//camera shake curve
	private MainCameraBehaviour mainCamera;
	public AnimationCurve cameraShakeCurve;
	private float cameraShakeCurveEndTime;

	//gun sounds
	private AudioSource gunAudioSource;
	public AudioClip[] gunShotSounds;

	//gun firing mechanics, reloading delays
	public int ammoSpentPerShot;

	[Tooltip("In seconds")]
	public float fireRate;
    private bool canShoot = true;

	[Tooltip("In seconds")]
	public float reloadRate;
	private bool reloading = false;

	[Range(0.0f, 30.0f)]
	public float scatteringAngle;

	public float projectileSpeed;

    public float projectileMinDamage;
    public float projectileMaxDamage;

	public int magazineSize;
	private int magazineCurrentSize;

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



		//debug: start will full magazine
		magazineCurrentSize = magazineSize;
	}

	private void Awake()
	{
		//find the last keyframe of the shake curve
		Keyframe lastKeyframe = cameraShakeCurve[cameraShakeCurve.length - 1];
		cameraShakeCurveEndTime = lastKeyframe.time;


		//TEMPORARY, BUG-PRONE
		projectileSource = GameObject.FindGameObjectWithTag("ProjectileSource");


		gunAudioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
	}

	void Update () {

		//shoot only if aiming and not reloading
		if (inUse && Input.GetKey(KeyCode.Mouse1) && Input.GetKeyDown(KeyCode.Mouse0) && canShoot && inventoryManager.CheckIfItemIsInInventory(4))
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
		Debug.Log(projectileSource.transform.position);
		Debug.Log(projectileSource.transform.eulerAngles);

		//sound! PogChamp
		AudioClip randomSFX = gunShotSounds[Random.Range(0, gunShotSounds.Length)];
		gunAudioSource.PlayOneShot(randomSFX);

		//apply camera shake
		mainCamera.cameraShakeCurve = cameraShakeCurve;
		mainCamera.cameraShakeCurveEndTime = cameraShakeCurveEndTime;
		mainCamera.cameraShakeCurrentTime = 0;

		//do... while loop so the code is ran at least once no matter the condition
		int i = 0;

		do { 
			GameObject newShot = Instantiate(projectilePrefab);

			//calculating damage
			newShot.GetComponent<DamageBehaviour>().damage = Random.Range(projectileMinDamage, projectileMaxDamage);

			newShot.gameObject.transform.position = projectileSource.transform.position;

			//apply random scattering factor
			Vector3 scattering = new Vector3(0, 0, Random.Range(-scatteringAngle, scatteringAngle));
			newShot.gameObject.transform.Rotate(projectileSource.transform.eulerAngles + scattering);

			newShot.GetComponent<Rigidbody2D>().AddForce(newShot.gameObject.transform.up * projectileSpeed);

			// (poor) ammo management
			magazineCurrentSize --;
			inventoryManager.RemoveItem(4);

			i++;

		} while ( i < ammoSpentPerShot);

		if (magazineCurrentSize <= 0)
		{
			//reload if possible
			canShoot = false;
			reloading = true;

			yield return new WaitForSeconds(reloadRate);


			//TODO: proper ammo reloading mechanics
			magazineCurrentSize = magazineSize;
			reloading = false;
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
