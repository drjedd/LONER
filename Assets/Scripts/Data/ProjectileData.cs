using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Scriptable Object hosts data from our throwable items like animation curves for the camera shake, scattering, fire rate, etc. */

[CreateAssetMenu(fileName = "Projectile", menuName = "LONER/Editor Tools/Create Projectile")]
public class ProjectileData : ScriptableObject {

	public GameObject projectilePrefab;

	public AnimationCurve cameraShakeCurve;


	//throwBar hold sounds
	public AudioClip throwBarHoldSound;

	//throw sounds
	public AudioClip[] throwSounds;


	//throwing mechanics, reloading delays
	public int ammoSpentPerThrow;

	[Tooltip("In seconds")]
	public float throwRate;

	[Range(0.0f, 100.0f)]
	public float scatteringAngle;

	public AnimationCurve throwStrengthCurve;

	public float minStrength;
	public float maxStrength;

	public float minDamage;
	public float maxDamage;

	public float minRotationSpeed;
	public float maxRotationSpeed;

}
