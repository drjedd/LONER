using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class GateBehaviour : MonoBehaviour {

	public bool gateIsOpen = true;
	public int destinationSceneIndex;

	// Use this for initialization
	void Start () {
		if (destinationSceneIndex < 0 || destinationSceneIndex > SceneManager.sceneCountInBuildSettings)
		{
			//scene is invalid: make the gate a wall instead by disabling "Trigger" mode on Collider
			GetComponent<Collider2D>().isTrigger = false;
		}
		else
		{
			// put here any condition (player level, has a key, etc.)
			if (gateIsOpen)
			{
				GetComponent<Collider2D>().isTrigger = true;
			}
			else
			{
				GetComponent<Collider2D>().isTrigger = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("TOUCHED");
		if (collider.gameObject == Player.Instance.gameObject) {
			Debug.Log("PLAYER INDEED, LET'S GO TO SCENE #" + destinationSceneIndex);
			SceneManager.LoadScene(destinationSceneIndex);

			Player.Instance.transform.position = new Vector3(3.5f, 3, 0);
		}
	}
}
