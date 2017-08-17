using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "LONER/Editor Tools/Create Gun")]
public class GunData : ScriptableObject {

	public GameObject projectilePrefab;

	public AnimationCurve cameraShakeCurve;

	//gun sounds
	public AudioClip[] gunShotSounds;

	//gun firing mechanics, reloading delays
	public int ammoSpentPerShot;

	[Tooltip("In seconds")]
	public float fireRate;

	[Tooltip("In seconds")]
	public float reloadRate;

	[Range(0.0f, 100.0f)]
	public float scatteringAngle;

	public float projectileSpeed;

	public float projectileMinDamage;
	public float projectileMaxDamage;

	public int magazineSize;
}
