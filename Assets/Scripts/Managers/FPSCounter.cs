using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

	//fps display
	public bool showFPS = true;
	public Text FPSlabel;
	private float FPScount;

	//fps calculation
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

			yield return new WaitForSeconds(0.5f);
		}
	}

}
