using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *	GAME MANAGER (Unity Singleton)
 *	
 *	Not used as of August 2017
 * 
 *
 */

public class GameManager : MonoBehaviour
{
	//Unity singleton instance
	protected static GameManager instance;
	public static GameManager Instance
	{
		get { return instance; }
	}
	
	void Awake()
	{
		//security checks
		if (instance != null)
		{
			Debug.LogWarning("Another instance of GameManager Singleton was detected, killing this one now");
			GameObject.Destroy(this.gameObject);
			return;
		}

		instance = this;
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
}

