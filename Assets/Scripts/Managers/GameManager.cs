using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//Unity singleton instance
	protected static GameManager instance;
	public static GameManager Instance
	{
		get { return instance; }
	}

	// Use this for initialization
	void Awake()
	{

		if (instance != null)
		{
			Debug.LogWarning("Another instance of GameManager Singleton was detected, killing this one now");
			GameObject.Destroy(this.gameObject);
			return;
		}

		instance = this;
		GameObject.DontDestroyOnLoad(this.gameObject);

		Application.targetFrameRate = 60;
	}

	// Update is called once per frame
	void Update()
	{

	}
}

