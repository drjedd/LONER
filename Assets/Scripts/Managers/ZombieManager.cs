using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ZombieManager : MonoBehaviour {

    public Text zombiesLeftUI;
    public Text statusTextUI;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        int amountOfEnemiesLeft = enemies.Length;

        zombiesLeftUI.text = "Zombies left: " + amountOfEnemiesLeft;

        if (amountOfEnemiesLeft == 0)
        {
            statusTextUI.text = "YOU WIN\nPress [R] to play again";
        }

	}
}
