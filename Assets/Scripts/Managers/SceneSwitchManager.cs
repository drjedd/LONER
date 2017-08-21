using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* SCENE SWITCHING MANAGER (Unity Singleton)
 * 
 * Takes note of where the player left the previous scene, loads the next one and tries to position the player as close as possible so it feels right
 * 
 * TODO: centralise here the code that is spread out in the GateBehaviour objects
 */

public class SceneSwitchManager : MonoBehaviour {

	protected static SceneSwitchManager instance;
	public static SceneSwitchManager Instance
	{
		get { return instance; }
	}

	public static GateBehaviour.Edge destinationEdge;
	public static float playerRelativePosition;
	
	void Start () {
		
		//Unity Singleton: if this manager already exists, destroy this instance
		if (instance != null)
		{
			Debug.LogWarning("Another instance of SceneSwitchManager was detected, killing this one now");
			GameObject.Destroy(this.gameObject);
			return;
		}

		instance = this;
		GameObject.DontDestroyOnLoad(this.gameObject);
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Delete))
		{
			//reset game (temp)
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

	}
}
