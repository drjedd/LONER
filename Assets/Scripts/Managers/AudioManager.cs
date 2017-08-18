using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour {

	public bool mutedInEditor = true;

	// Use this for initialization
	void Start () {

		//mute if working on game (for now)
		#if UNITY_EDITOR
		if (mutedInEditor)
		{
			AudioListener.volume = 0;
		}
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
