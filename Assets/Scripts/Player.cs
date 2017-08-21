using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * PLAYER (Unity Singleton, not destroyed on Scene Change)
 * 
 * Does not do much else at the moment ResidentSleeper
 * 
 */

public class Player: MonoBehaviour {

	protected static Player instance;
	public static Player Instance 
	{
		get { return instance; }
	}
	
	void Awake () {
		
		if ( instance != null )
		{
			Debug.LogWarning("Another instance of PlayerSingleton was detected, killing this one now");
			GameObject.Destroy(this.gameObject);
			return;
		}

		instance = this;
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
}
