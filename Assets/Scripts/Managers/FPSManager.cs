using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSManager : MonoBehaviour {

	//FPS display
	public int targetFramerate = 60;

	public bool showFPS = true;
	public Text FPSlabel;
	private float FPScount;

	//FPS calculation
	IEnumerator Start()
	{
		GUI.depth = 2;
		while (true)
		{
			if (Time.timeScale == 0)
			{
				FPSlabel.text = "Paused";
			}
			else
			{
				yield return new WaitForSeconds(0.1f);
				FPScount = (1 / Time.deltaTime);
				FPSlabel.text = "FPS: " + (Mathf.Round(FPScount));
			}

			//update twice per second so it's readable
			yield return new WaitForSeconds(0.5f);
		}
	}

	private void Awake()
	{
		Application.targetFrameRate = targetFramerate;
	}

}
