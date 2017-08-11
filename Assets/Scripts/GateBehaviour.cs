﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class GateBehaviour : MonoBehaviour {

	public bool gateIsOpen = true;
	public int destinationSceneIndex;

	private Vector2 playerRelativePosition;
	private Bounds gateBounds;

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

		//get gate bounds to calculate player relative position
		gateBounds = GetComponent<Renderer>().bounds;
		
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		
		if (collider.gameObject == Player.Instance.gameObject) {

			//currently does not work. The idea is to get the player position relative to the gate so we can use that in the next scene
			playerRelativePosition = new Vector2(Mathf.InverseLerp(gateBounds.min.x, gateBounds.max.x, collider.transform.position.x), Mathf.InverseLerp(gateBounds.min.y, gateBounds.max.y, collider.transform.position.y));

			Debug.Log(playerRelativePosition);

			Debug.Log("LOADING SCENE #" + destinationSceneIndex);
			SceneManager.LoadScene(destinationSceneIndex);

			//debug: player (and camera) position "reset"
			Player.Instance.transform.position = new Vector3(3.5f, 3, 0);
		}
	}
}
