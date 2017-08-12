using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class GateBehaviour : MonoBehaviour {

	public bool gateIsOpen = true;
	public int destinationSceneIndex;

	private float playerRelativePosition;
	private Bounds gateBounds;

	private Vector3 destinationPosition;

	//used to define which edge this is
	public Edge edgeType;
	public enum Edge {
		Top,
		Bottom,
		Left,
		Right
	}

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
		gateBounds = GetComponent<Collider2D>().bounds;

		//shitty debug
		if (SceneSwitchManager.playerRelativePosition != 0)
		{
			if (SceneSwitchManager.destinationEdge == edgeType)
			{
				//debug: player (and camera) position "reset"
				//Player.Instance.transform.position = new Vector3(3.5f, 3, 0);

				switch (edgeType)
				{
					case Edge.Left:
						destinationPosition = new Vector3(gateBounds.max.x + Const.SCENE_SWITCH_PADDING, Mathf.Lerp(gateBounds.min.y, gateBounds.max.y, SceneSwitchManager.playerRelativePosition));
						break;

					case Edge.Right:
						destinationPosition = new Vector3(gateBounds.min.x - Const.SCENE_SWITCH_PADDING, Mathf.Lerp(gateBounds.min.y, gateBounds.max.y, SceneSwitchManager.playerRelativePosition));
						break;

					case Edge.Bottom:
						destinationPosition = new Vector3(Mathf.Lerp(gateBounds.min.x, gateBounds.max.x, SceneSwitchManager.playerRelativePosition), gateBounds.max.y + Const.SCENE_SWITCH_PADDING);
						break;

					case Edge.Top:
						destinationPosition = new Vector3(Mathf.Lerp(gateBounds.min.x, gateBounds.max.x, SceneSwitchManager.playerRelativePosition), gateBounds.min.y - Const.SCENE_SWITCH_PADDING);
						break;

				}

				Player.Instance.transform.position = destinationPosition;




				//BELOW: debug, fixing messy camera jitter due to smoothing
				GameObject mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");

				mainCameraObject.GetComponent<MainCameraBehaviour>().SetPosition(destinationPosition);

				//Debug.Log("SPAWNED PLAYER AT " + edgeType + " EDGE (" + Player.Instance.gameObject.transform.position + ")");
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		
		if (collider.gameObject == Player.Instance.gameObject) {


			if (edgeType == Edge.Left || edgeType == Edge.Right)
			{	
				playerRelativePosition = Mathf.InverseLerp(gateBounds.min.y, gateBounds.max.y, collider.transform.position.y);
			
				if (edgeType == Edge.Left)
			
				{
					SceneSwitchManager.destinationEdge = Edge.Right;
				}
				else
				{
					SceneSwitchManager.destinationEdge = Edge.Left;
				}
			}
			else
			{
				playerRelativePosition = Mathf.InverseLerp(gateBounds.min.x, gateBounds.max.x, collider.transform.position.x);

				if (edgeType == Edge.Top)

				{
					SceneSwitchManager.destinationEdge = Edge.Bottom;
				}
				else
				{
					SceneSwitchManager.destinationEdge = Edge.Top;
				}
			}

			//store the relative position in the switch manager because this object is going to be destroyed (RIP)
			SceneSwitchManager.playerRelativePosition = playerRelativePosition;

			//Debug.Log("LOADING SCENE #" + destinationSceneIndex);
			SceneManager.LoadScene(destinationSceneIndex);
		}
	}
}
