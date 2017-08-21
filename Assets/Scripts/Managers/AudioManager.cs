using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour {

	public bool mutedInEditor = true;
	
	void Start () {

		//easy mute if working long hours or with music (for now)
		#if UNITY_EDITOR
		if (mutedInEditor)
		{
			AudioListener.volume = 0;
		}
		#endif
	}
}
