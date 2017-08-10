using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour {

	protected static Player instance;
	public static Player Instance 
	{
		get { return instance; }
	}

	// Use this for initialization
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
	
	// Update is called once per frame
	void Update () {
		
	}
}
