using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "LONER/Editor Tools/Create Projectile")]
public class ProjectileData : ScriptableObject {

	public GameObject projectilePrefab;

	public AnimationCurve cameraShakeCurve;

	//gun sounds
	public AudioClip[] throwSounds;

	//gun firing mechanics, reloading delays
	public int ammoSpentPerThrow;

	[Tooltip("In seconds")]
	public float throwRate;

	[Range(0.0f, 100.0f)]
	public float scatteringAngle;

	public AnimationCurve throwStrengthCurve;

}
