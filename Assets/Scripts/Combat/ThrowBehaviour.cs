﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ThrowBehaviour : MonoBehaviour
{

	public ProjectileData ProjectileData;

	public bool inUse = false;
	private bool canShoot = true;

	private InventoryManager inventoryManager;

	//source of the projectile (limited to right hand for now)
	private GameObject projectileSource;

	//camera shake curve
	private MainCameraBehaviour mainCamera;
	private float cameraShakeCurveEndTime;

	//throw power bar
	public GameObject throwBarUI;

	private float throwStrengthCurveEndTime;
	public float throwStrength;

	public float throwStrengthCurveStartTime;

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
		Keyframe lastKeyframeShake = ProjectileData.cameraShakeCurve[ProjectileData.cameraShakeCurve.length - 1];
		cameraShakeCurveEndTime = lastKeyframeShake.time;

		//find the last keyframe of the throw strength curve
		Keyframe lastKeyframeThrow = ProjectileData.throwStrengthCurve[ProjectileData.throwStrengthCurve.length - 1];
		throwStrengthCurveEndTime = lastKeyframeThrow.time;


		//TEMPORARY, BUG-PRONE
		projectileSource = GameObject.FindGameObjectWithTag("ProjectileSource");

		throwBarUI = GameObject.FindGameObjectWithTag("ThrowPercentage");

		gunAudioSource = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();

	}

	void Update()
	{

		//enable gun only if item is equipped
		inUse = GetComponent<UIItem>().inUse;

		//shoot only if aiming and not reloading
		if (inUse && Input.GetKey(KeyCode.Mouse1) && canShoot && inventoryManager.CheckIfItemIsInInventory(1))
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				throwStrength = .1f;
				throwStrengthCurveStartTime = Time.time;
			}
			else if (Input.GetKey(KeyCode.Mouse0))
			{
				Debug.Log("UP AND UP");
				throwStrength += Time.time - throwStrengthCurveStartTime;

				Vector3 tempVector3 = new Vector3(throwStrength, 1f, 1f);
				throwBarUI.GetComponent<RectTransform>().localScale = tempVector3;
			}
			else if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				Debug.Log("RELEASE");
				StartCoroutine(Throw(throwStrength));
			}
		}
	}

	IEnumerator Throw(float strength)
	{
		//sound! PogChamp
		if (ProjectileData.throwSounds.Length > 0)
		{
			AudioClip randomSFX = ProjectileData.throwSounds[Random.Range(0, ProjectileData.throwSounds.Length)];
			gunAudioSource.PlayOneShot(randomSFX);
		}

		//apply camera shake
		mainCamera.cameraShakeCurve = ProjectileData.cameraShakeCurve;
		mainCamera.cameraShakeCurveEndTime = cameraShakeCurveEndTime;
		mainCamera.cameraShakeCurrentTime = 0;

		//do... while loop so the code is ran at least once no matter the condition
		int i = 0;

		do
		{
			GameObject newShot = Instantiate(ProjectileData.projectilePrefab);

			//calculating damage based on power curve
			//newShot.GetComponent<DamageBehaviour>().damage = Random.Range(ProjectileData.projectileMinDamage, ProjectileData.projectileMaxDamage);

			newShot.gameObject.transform.position = projectileSource.transform.position;

			//define bullet base rotation
			newShot.gameObject.transform.up = Camera.main.ScreenToWorldPoint(Input.mousePosition) - newShot.gameObject.transform.position;


			//apply random scattering factor
			Vector3 scattering = new Vector3(0, 0, Random.Range(-ProjectileData.scatteringAngle, ProjectileData.scatteringAngle));

			Debug.Log("Scattering: " + scattering.z);
			newShot.gameObject.transform.Rotate(scattering);

			//newShot.gameObject.transform.LookAt(mainCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition));

			newShot.GetComponent<Rigidbody2D>().AddForce((newShot.gameObject.transform.up) * 10f); //ProjectileData.projectileSpeed);

			Physics2D.IgnoreCollision(newShot.GetComponent<Collider2D>(), Player.Instance.gameObject.GetComponent<Collider2D>(), true);



			//BulletBehaviour bulletBehaviour = newShot.GetComponent<BulletBehaviour>();

			//bulletBehaviour.bulletSpeed = GunData.projectileSpeed;

			////debug: assigning Player as default owner
			//bulletBehaviour.owner = Player.Instance.gameObject;

			// (poor) ammo management
			magazineCurrentSize--;
			inventoryManager.RemoveItem(4);

			i++;

		} while (i < ProjectileData.ammoSpentPerThrow);
		
		//firing delay
		canShoot = false;
		yield return new WaitForSeconds(ProjectileData.throwRate);

		canShoot = true;
	}
}
